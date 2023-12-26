using FormatConverter.Web.Models;
using FormatConverter.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormatConverter.Web.Controllers;

[Authorize(Roles = "admin")]
public class AdminController : Controller
{
    private readonly IModuleService _moduleService;
    private readonly IPermissionService _permissionService;
    private readonly UserManager<IdentityUser> _userManager;

    public AdminController(IModuleService moduleLoader, UserManager<IdentityUser> userManager, IPermissionService permissionService)
    {
        _moduleService = moduleLoader;
        _permissionService = permissionService;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        var adminActions = new List<AdminActionVm>()
        {
            new(nameof(Permissions), Resources.AdminPermissionsOptionTitle),
            new(nameof(Register), Resources.AdminUserRegistrationTitle),
            new(nameof(Modules), Resources.AdminModulesOptionTitle),
        };

        return View(adminActions);
    }

    [HttpGet]
    public IActionResult Modules()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Modules(IFormFile moduleFile)
    {
        if (moduleFile.Length == 0 || Path.GetExtension(moduleFile.FileName) != ".dll")
        {
            ModelState.AddModelError(string.Empty, Resources.ModuleFileWarningMessage);
            return View();
        }
        if (_moduleService.ModuleExists(moduleFile))
        {
            ModelState.AddModelError(string.Empty, Resources.ModuleExistenceWarningMessage);
            return View();
        }

        TempData["Success"] = "Succesfully loaded.";
        _moduleService.LoadConverterModule(moduleFile);
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Permissions(PermissionsVm? permissionsVm)
    {
        if (string.IsNullOrEmpty(permissionsVm?.UserEmail))
            return View();

        var user = await _userManager.FindByEmailAsync(permissionsVm.UserEmail);
        if (user is null)
        {
            ModelState.AddModelError(string.Empty, Resources.NonexistentEmailWarningMessage);
            return View();
        }

        var converters = _moduleService.GetConverterModules();
        permissionsVm.Permissions = 
            from converter in converters
            let converterName = converter.GetType().Name
            select new SelectListItem(converterName, converterName);
        foreach (var selectListItem in permissionsVm.Permissions)
            selectListItem.Selected = await _permissionService.UserHasPermission(user, selectListItem.Value);

        return View(permissionsVm);
    }

    [HttpPost]
    public async Task<IActionResult> PermissionsPost(PermissionsVm permissionsVm)
    {
        var userEmail = permissionsVm.UserEmail;
        if (string.IsNullOrEmpty(userEmail))
            throw new InvalidOperationException("User email not set.");

        var selectedClaims = permissionsVm.ToggledPermissions ?? Enumerable.Empty<string>();
        await _permissionService.AddPermissionsToUser(userEmail, selectedClaims);
        var unselectedClaims = _moduleService.GetConverterClaims();
        unselectedClaims.ExceptWith(selectedClaims);
        await _permissionService.RemovePermissionsFromUser(userEmail, unselectedClaims);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Register()
    {
        return RedirectToAction("Register", "Authentication", new { returnUrl = Url.Action(nameof(Index), "Admin") });
    }
}
