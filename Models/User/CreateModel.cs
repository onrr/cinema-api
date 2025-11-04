using System.ComponentModel.DataAnnotations;

namespace Cinema.Models;

public class CreateModel
{
    [Required]
    [MinLength(3, ErrorMessage = "Minimum 3 characters")]
    public string FirstName { get; set; } = null!;
    [Required]
    [MinLength(3, ErrorMessage = "Minimum 3 characters")]
    public string LastName { get; set; } = null!;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = null!;
}