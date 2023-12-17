using Microsoft.AspNetCore.Identity;

namespace ECommerce.WebApi.Models
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string? Address { get; set; }
        public string? Role { get; set; }
    }
}
