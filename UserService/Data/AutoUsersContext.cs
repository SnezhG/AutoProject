using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.Data
{
    public class AutoUsersContext: IdentityDbContext
    {

        public AutoUsersContext(DbContextOptions options) : base(options)
        { 
        
        }

        public DbSet<AutoUser> AutoUsers { get; set; }

    }
}
