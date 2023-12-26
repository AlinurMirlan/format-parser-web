using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FormatConverter.Web.Models;

public class PermissionsVm
{
    [EmailAddress]
    public string? UserEmail { get; set; }
    public IEnumerable<string>? ToggledPermissions { get; set; }
    public IEnumerable<SelectListItem> Permissions { get; set; } = Enumerable.Empty<SelectListItem>();
}
