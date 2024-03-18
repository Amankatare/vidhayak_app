using Microsoft.EntityFrameworkCore;
using VidhayakApp.Core.Entities;

namespace VidhayakApp.Infrastructure.Data
{
    public class VidhayakAppContext : DbContext
    {
        public VidhayakAppContext(DbContextOptions<VidhayakAppContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Ward> Wards { get; set; }
        public DbSet<UserDetail> UserDetails {  get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Item> Items { get; set; }
    
        public DbSet<Communication> Communications { get; set; }
        public DbSet<GovtDepartment> GovtDepartments { get; set; }
        public DbSet<GovtScheme> GovtSchemes{ get; set; }

        // Add DbSet properties for other entities

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure entity relationships and properties here
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Dob).HasColumnType("date");
                entity.Property(e => e.Address).IsRequired();
                entity.Property(e => e.Ward).IsRequired();
                entity.Property(e => e.MobileNumber).IsRequired();
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PasswordHash).IsRequired();
                //entity.Property(e => e.Password).IsRequired();
                entity.Ignore(e => e.Ward);
            });
            /*
                modelBuilder.Entity<UserDetail>(entity =>
                {
                    entity.HasKey(e => e.DetailId);
                    entity.Property(e => e.Education).IsRequired();
                    entity.Property(e => e.AadharNumber).IsRequired();
                    entity.Property(e => e.SamagraID).IsRequired();
                    entity.Property(e => e.VoterID).IsRequired();
                    entity.Property(e => e.Caste).IsRequired();

                    // Define the foreign key relationship
                    entity.HasOne(e => e.User)
                          .WithMany(u => u.UserDetails)
                          .HasForeignKey(e => e.UserId)
                          .OnDelete(DeleteBehavior.Cascade);
            */
                    // Add configuration for other properties and relationships as necessary
               // });

                // Add configuration for other entities

            
        }
    }
}
