using Microsoft.AspNetCore.Identity;

namespace Cinema.Models;

public class AppUser : IdentityUser<int>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}
