using AutoMapper;
using FormatConverter.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FormatConverter.Web.Controllers;

public class AuthenticationController : Controller
{
    private readonly ILogger logger;
    private readonly UserManager<IdentityUser> userManager;
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly IMapper mapper;

    public AuthenticationController(ILogger<AuthenticationController> logger, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IMapper mapper)
    {
        this.logger = logger;
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.mapper = mapper;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(CredentialsVm credentials)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        var user = await userManager.FindByEmailAsync(credentials.Email);
        if (user is null)
        {
            ModelState.AddModelError("", Resources.LoginFailedWarningMessage);
            logger.LogInformation("Attempted login with a non-existent email: {email}", credentials.Email);
            return View();
        }

        var result = await signInManager.PasswordSignInAsync(user, credentials.Password, credentials.RememberMe, false);
        if (!result.Succeeded)
        {
            ModelState.AddModelError("", Resources.LoginFailedWarningMessage);
            logger.LogInformation("Attempted login with a wrong password for the email: {email}", credentials.Email);
            return View();
        }

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    public IActionResult Register(string? returnUrl)
    {
        return View(new RegistrationVm() { ReturnUrl = returnUrl });
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegistrationVm credentials)
    {
        if (!ModelState.IsValid)
        {
            return View(credentials);
        }
        
        var user = mapper.Map<IdentityUser>(credentials);
        var userWithTheSameEmail = await userManager.FindByEmailAsync(user.Email!);
        if (userWithTheSameEmail is not null)
        {
            ModelState.AddModelError(nameof(RegistrationVm.Email), Resources.EmailTakenWarningMessage);
            return View(credentials);
        }

        var result = await userManager.CreateAsync(user, credentials.Password!);
        if (!result.Succeeded)
        {
            logger.LogError("Something went wrong in the registration process.");
            ModelState.AddModelError("", Resources.UnknownErrorWarningMessage);
            return View(credentials);
        }

        if (!string.IsNullOrEmpty(credentials.ReturnUrl))
            return Redirect(credentials.ReturnUrl);

        await signInManager.SignInAsync(user, isPersistent: credentials.RememberMe);
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction(nameof(Index));
    }
}
