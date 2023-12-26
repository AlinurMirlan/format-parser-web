using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace FormatConverter.Web.Services;

public interface IPermissionService
{
    public Task AddPermissionsToUser(string userEmail, IEnumerable<string> permissions);
    public Task<bool> UserHasPermission(IdentityUser user, string permission);
    public bool IsAdministrator(ClaimsPrincipal user);
    public Task RemovePermissionsFromUser(string userEmail, IEnumerable<string> permissions);
    public bool HasPermission(ClaimsPrincipal user, string permission);
}