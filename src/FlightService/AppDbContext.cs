using FlightService.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlightService
{
    public partial class AppDbContext : DbContext
    {

        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Airport> Airports { get; set; } = null!;
        public virtual DbSet<Flight> Flights { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Airport>(entity =>
            {
                entity.ToTable("airport");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.City)
                    .HasMaxLength(255)
                    .HasColumnName("city");

                entity.Property(e => e.Country)
                    .HasMaxLength(255)
                    .HasColumnName("country");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Flight>(entity =>
            {
                entity.ToTable("flight");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Datetime).HasColumnName("datetime");

                entity.Property(e => e.Flightnumber)
                    .HasMaxLength(20)
                    .HasColumnName("flightnumber");

                entity.Property(e => e.Fromairportid).HasColumnName("fromairportid");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Toairportid).HasColumnName("toairportid");

                entity.HasOne(d => d.Fromairport)
                    .WithMany(p => p.FlightFromairports)
                    .HasForeignKey(d => d.Fromairportid)
                    .HasConstraintName("flight_fromairportid_fkey");

                entity.HasOne(d => d.Toairport)
                    .WithMany(p => p.FlightToairports)
                    .HasForeignKey(d => d.Toairportid)
                    .HasConstraintName("flight_toairportid_fkey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
