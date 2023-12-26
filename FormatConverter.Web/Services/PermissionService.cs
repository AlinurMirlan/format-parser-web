using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace FormatConverter.Web.Services;

public class PermissionService : IPermissionService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly string _adminRoleString;

    public PermissionService(UserManager<IdentityUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _adminRoleString = configuration["Security:Roles:Admin"] ?? throw new InvalidOperationException("Admin Role is not set up.");
    }

    public bool IsAdministrator(ClaimsPrincipal user)
        => user.IsInRole(_adminRoleString);

    public bool HasPermission(ClaimsPrincipal user, string permission)
        => user.HasClaim(permission, permission);

    public async Task<bool> UserHasPermission(IdentityUser user, string permission)
    {
        var claims = await _userManager.GetClaimsAsync(user);
        bool hasPermission = claims.Where(c => c.Type == permission).Any();
        return hasPermission;
    }

    public async Task AddPermissionsToUser(string userEmail, IEnumerable<string> permissions)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);

        if (user != null)
        {
            var claims = permissions.Select(p => new Claim(p, p));
            await _userManager.AddClaimsAsync(user, claims);
        }
    }

    public async Task RemovePermissionsFromUser(string userEmail, IEnumerable<string> permissions)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);

        if (user != null)
        {
            var claims = permissions.Select(p => new Claim(p, p));
            await _userManager.RemoveClaimsAsync(user, claims);
        }
    }
}
