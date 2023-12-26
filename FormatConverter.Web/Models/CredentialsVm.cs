using System.ComponentModel.DataAnnotations;

namespace FormatConverter.Web.Models;

public class CredentialsVm
{
    [EmailAddress]
    [Required]
    public required string Email { get; set; }

    [Required]
    [MinLength(8, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "PasswordLengthErrorMessage")]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    public bool RememberMe { get; set; }
}
