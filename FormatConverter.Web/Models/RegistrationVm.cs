using System.ComponentModel.DataAnnotations;

namespace FormatConverter.Web.Models;

public class RegistrationVm
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "PasswordLengthErrorMessage")]
    public string? Password { get; set; }

    [Display(ResourceType = typeof(Resources), Name = "PasswordConfirmationName")]
    [Compare(nameof(Password), ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "PasswordMismatchErrorMessage")]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "PasswordLengthErrorMessage")]
    public string? ConfirmPassword { get; set; }

    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
}
