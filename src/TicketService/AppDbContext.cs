using Microsoft.EntityFrameworkCore;
using TicketService.Entities;

namespace TicketService
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Ticket> Tickets { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.ToTable("ticket");

                entity.HasIndex(e => e.Ticketuid, "ticket_ticketuid_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Flightnumber)
                    .HasMaxLength(20)
                    .HasColumnName("flightnumber");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .HasColumnName("status");

                entity.Property(e => e.Ticketuid).HasColumnName("ticketuid");

                entity.Property(e => e.Username)
                    .HasMaxLength(80)
                    .HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
