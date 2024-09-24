using AuthService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService
{
    public partial class AppDbContext : DbContext
    {

        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<User> User { get; set; } = null!;
    }
}

