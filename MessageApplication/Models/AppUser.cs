using Microsoft.AspNetCore.Identity;

namespace MessageApplication.Models
{
    public class AppUser:IdentityUser<int>
    {
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime UpdateAt { get; set; } = DateTime.Now;
    }
}
