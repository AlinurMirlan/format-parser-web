using FormatConverter.Library;
using System.Reflection;
using System.Security.Claims;

namespace FormatConverter.Web.Services;

public class ModuleService : IModuleService
{
    private readonly string _modulesFolder;
    private readonly IPermissionService _permissionsService;

    public ModuleService(IConfiguration configuration, IPermissionService permissionsService)
    {
        _permissionsService = permissionsService;
        var key = "Directories:ModulesDirectory";
        _modulesFolder = configuration[key] ?? throw new ArgumentNullException(key, $"Configuration key is absent.");
    }

    public void LoadConverterModule(IFormFile moduleFile)
    {
        var moduleFilePath = Path.Combine(_modulesFolder, moduleFile.FileName);
        if (File.Exists(moduleFilePath))
            throw new IOException("Given module already exists.");

        using var fileStream = File.OpenWrite(moduleFilePath);
        moduleFile.CopyTo(fileStream);
    }

    public bool ModuleExists(IFormFile moduleFile)
    {
        var moduleFilePath = Path.Combine(_modulesFolder, moduleFile.FileName);
        return File.Exists(moduleFilePath);
    }

    public HashSet<string> GetConverterClaims() 
    {
        var roles = new HashSet<string>();
        var formatConverters = GetConverterModules();
        foreach (var converter in formatConverters)
            roles.Add(converter.GetType().Name);
        
        return roles;
    }

    public IEnumerable<IFormatConverter> GetAllowedConverterModules(ClaimsPrincipal user)
    {
        var allowedFormatConverters = new LinkedList<IFormatConverter>();
        var formatConverters = GetConverterModules();
        if (_permissionsService.IsAdministrator(user))
            return formatConverters;

        foreach (var formatConverter in formatConverters)
            {
            if (_permissionsService.HasPermission(user, GetConverterClaim(formatConverter)))
                allowedFormatConverters.AddLast(formatConverter);
        }

        return allowedFormatConverters;
    }

    private static string GetConverterClaim(IFormatConverter formatConverter) =>
        formatConverter.GetType().Name;

    public IEnumerable<IFormatConverter> GetConverterModules()
    {
        var formatConverters = new LinkedList<IFormatConverter>();
        foreach (var assemblyFilePath in Directory.EnumerateFiles(_modulesFolder, "*.dll"))
        {
            var assembly = Assembly.LoadFrom(assemblyFilePath);
            foreach (var type in assembly.GetExportedTypes())
            {
                if (type.IsClass && type.IsAssignableTo(typeof(IFormatConverter)))
                {
                    var formatConverter = (IFormatConverter?)Activator.CreateInstance(type) 
                                            ?? throw new InvalidCastException();
                    formatConverters.AddLast(formatConverter);
                }
            }
        }

        return formatConverters;
    }
}
