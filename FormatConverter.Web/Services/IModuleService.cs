using FormatConverter.Library;
using System.Security.Claims;

namespace FormatConverter.Web.Services;

public interface IModuleService
{
    public IEnumerable<IFormatConverter> GetConverterModules();
    public HashSet<string> GetConverterClaims();
    public IEnumerable<IFormatConverter> GetAllowedConverterModules(ClaimsPrincipal user);
    public void LoadConverterModule(IFormFile moduleFile);
    public bool ModuleExists(IFormFile moduleFile);
}