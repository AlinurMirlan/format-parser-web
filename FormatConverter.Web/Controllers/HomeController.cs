using AutoMapper;
using FormatConverter.Library;
using FormatConverter.Web.Models;
using FormatConverter.Web.Services;
using FormatParser.Library.Formats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Mime;
using SystemFile = System.IO.File;

namespace FormatConverter.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IMapper _mapper;
    private readonly IModuleService _moduleLoader;
    private readonly IConfiguration _configuration;
    private readonly IZipService _zipService;

    public HomeController(IModuleService moduleLoader, IMapper mapper, IConfiguration configuration, IZipService zipService)
    {
        _mapper = mapper;
        _moduleLoader = moduleLoader;
        _configuration = configuration;
        _zipService = zipService;
    }

    public IActionResult Index()
    {
        var formatConverters = _moduleLoader.GetAllowedConverterModules(User);
        var converterVms = _mapper.Map<List<ConverterVm>>(formatConverters);
        converterVms.Add(DefaultConverterVm());
        return View(converterVms);
    }

    [HttpGet]
    public IActionResult Converter(string? converterClassName)
    {
        var formatConverters = _moduleLoader.GetConverterModules();
        var chosenConverter = formatConverters
            .Where(converter => converter.GetType().Name == converterClassName).FirstOrDefault();
        ConverterVm converterVm = chosenConverter is null ? DefaultConverterVm() : _mapper.Map<ConverterVm>(chosenConverter);
        return View(converterVm);
    }

    [HttpPost]
    public IActionResult Converter(ConverterVm converterVm, IFormFile formFile)
    {
        if (formFile == null || formFile.Length == 0 || Path.GetExtension(formFile.FileName) != ".zip")
        {
            ModelState.AddModelError(string.Empty, Resources.ZipFileWarningMessage);
            return View(converterVm);
        }

        var key = "Directories:FormatUploadsDirectory";
        var formatUploadsDirectory = _configuration[key] ?? throw new ArgumentNullException(key, $"Configuration key is absent.");
        var formatContentDirectory = Path.Combine(formatUploadsDirectory, Path.GetFileNameWithoutExtension(formFile.FileName));
        _zipService.ExtractZipArchive(formFile.OpenReadStream(), formatContentDirectory);
        var formatConverters = _moduleLoader.GetAllowedConverterModules(User);
        if (!formatConverters.Any())
        {
            ModelState.AddModelError(string.Empty, Resources.ZipFileWarningMessage);
            return View(converterVm);
        }

        var chosenConverter = formatConverters
            .Where(converter => converter.GetType().Name == converterVm.ConverterClassName).FirstOrDefault();

        var convertedFormatFile = Path.Combine(formatUploadsDirectory, formFile.FileName);
        var converterConverion = (IFormatConverter converter) => _zipService.CreateZipArchive(convertedFormatFile,
                    stream => converter.Convert(new(formatContentDirectory), stream));

        var unsuccessfullConvesion = chosenConverter is not null
            ? EnumerateConversions(converterConverion, new[] { chosenConverter }) 
            : EnumerateConversions(converterConverion, formatConverters);
        if (unsuccessfullConvesion)
        {
            ModelState.AddModelError(string.Empty, Resources.InvalidFormatWarningMessage);
            return View(converterVm);
        }

        return File(SystemFile.OpenRead(convertedFormatFile), MediaTypeNames.Application.Zip, formFile.FileName);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorVm { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private static ConverterVm DefaultConverterVm() => new()
    {
        ConverterClassName = string.Empty,
        ConvertFrom = Resources.ConversionInferenceName,
        ConvertTo = Resources.ConversionTargetFormat
    };

    private static bool EnumerateConversions(Action<IFormatConverter> conversion, IEnumerable<IFormatConverter> formatConverters)
    {
        bool unsuccessfullConvesion = true;
        foreach (var formatConverter in formatConverters)
        {
            try
            {
                conversion(formatConverter);
                unsuccessfullConvesion = false;
                break;
            }
            catch (Exception exception) when (exception is InvalidFormatException ||
                                             exception is FileNotFoundException)
            {
                continue;
            }
        }

        return unsuccessfullConvesion;
    }
}
