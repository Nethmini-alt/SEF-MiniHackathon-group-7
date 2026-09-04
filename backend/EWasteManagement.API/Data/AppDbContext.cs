using Microsoft.EntityFrameworkCore;
using EWasteManagement.API.Models;

namespace EWasteManagement.API.Data
{
    /// <summary>
    /// Database context for the EWaste Management application
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// DbSet for Collection Centers
        /// </summary>
        public DbSet<Center> Centers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Center entity
            modelBuilder.Entity<Center>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.District)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.AcceptedItemType)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.OpeningHours)
                    .IsRequired()
                    .HasMaxLength(100);

                // Add an index on Name for faster searches
                entity.HasIndex(e => e.Name)
                    .HasDatabaseName("IX_Centers_Name");

                // Add an index on District for faster filtering
                entity.HasIndex(e => e.District)
                    .HasDatabaseName("IX_Centers_District");
            });
        }
    }
}