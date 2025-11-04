using System.ComponentModel.DataAnnotations;

namespace Cinema.Models;

public class LoginModel
{
    [Required]
    [Display(Name = "E-Mail")]
    [EmailAddress]
    public string Email { get; set; } = null!;
    [Required]
    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

}