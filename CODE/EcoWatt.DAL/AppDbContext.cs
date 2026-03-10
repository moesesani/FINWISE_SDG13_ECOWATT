using Microsoft.EntityFrameworkCore;
using EcoWatt.Models;

namespace EcoWatt.DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<EnergyLog> EnergyLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // EKSAKTONG PATH: Lalabas sa bin/Debug/net10.0-windows, 
            // lalabas sa EcoWatt.UI, lalabas sa CODE, 
            // papasok sa INPUT_DATA para mahanap ang database.
            optionsBuilder.UseSqlite("Data Source=../../../INPUT_DATA/inventory.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuration para sa User table
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Username).IsRequired();

                // Security: PasswordHash is required for SHA256 security (NFR2)
                entity.Property(u => u.PasswordHash).IsRequired();
            });

            // Configuration para sa EnergyLog at Relationship (1-to-Many)
            modelBuilder.Entity<EnergyLog>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Relationship: 1 User can have Many EnergyLogs
                entity.HasOne<User>()
                      .WithMany(u => u.EnergyLogs)
                      .HasForeignKey(e => e.UserId);
            });
        }
    }
}