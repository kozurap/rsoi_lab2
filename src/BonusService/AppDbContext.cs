using Microsoft.EntityFrameworkCore;
using PrivilegeService.Entiies;

namespace PrivilegeService
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

        public virtual DbSet<Privilege> Privileges { get; set; } = null!;
        public virtual DbSet<PrivilegeHistory> PrivilegeHistories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Privilege>(entity =>
            {
                entity.ToTable("privilege");

                entity.HasIndex(e => e.Username, "privilege_username_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Balance).HasColumnName("balance");

                entity.Property(e => e.Status)
                    .HasMaxLength(80)
                    .HasColumnName("status")
                    .HasDefaultValueSql("'BRONZE'::character varying");

                entity.Property(e => e.Username)
                    .HasMaxLength(80)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<PrivilegeHistory>(entity =>
            {
                entity.ToTable("privilege_history");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BalanceDiff).HasColumnName("balance_diff");

                entity.Property(e => e.Datetime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("datetime");

                entity.Property(e => e.OperationType)
                    .HasMaxLength(20)
                    .HasColumnName("operation_type");

                entity.Property(e => e.PrivilegeId).HasColumnName("privilege_id");

                entity.Property(e => e.TicketUid).HasColumnName("ticket_uid");

                entity.HasOne(d => d.Privilege)
                    .WithMany(p => p.PrivilegeHistories)
                    .HasForeignKey(d => d.PrivilegeId)
                    .HasConstraintName("privilege_history_privilege_id_fkey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }


}
