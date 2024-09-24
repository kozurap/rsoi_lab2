using Microsoft.EntityFrameworkCore;
using StatsService.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace StatsService
{
    public partial class AppDbContext : DbContext
    {

        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Stat> Stats { get; set; } = null!;
    }
}
