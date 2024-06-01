using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FindDoctor.Models;
using Models;

namespace FindDoctor.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<DoctorModel> Doctors { get; set; }
        public DbSet<PatientModel> Customers { get; set; }
        public DbSet<PatientDescriptionDetection> DescriptionDetections { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
          

            modelBuilder.Entity<PatientModel>()
                .HasMany(p => p.DescriptionDetections)
                .WithOne(d => d.Patient)
                .HasForeignKey(d => d.PatientId);
        }
    }
}
