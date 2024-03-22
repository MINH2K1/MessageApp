using MessageApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MessageApplication.DbContext
{
    public class MessageContext:IdentityDbContext<AppUser,IdentityRole<int>,int>
    {
        public MessageContext(
            DbContextOptions<MessageContext> options):base(options)
        {
        }
        DbSet<Group> Groups { get; set; }
        DbSet<Message> Messages { get; set; }
        DbSet<GroupUser> GroupUsers { get; set; }

    }
}
