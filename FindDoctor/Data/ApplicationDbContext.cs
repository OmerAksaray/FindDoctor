using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using FindDoctor.Models;
using Microsoft.EntityFrameworkCore;

namespace FindDoctor.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<DoctorModel> Doctors { get; set; }
        public DbSet<PatientModel> Customers { get; set; }
        public DbSet<PatientDescriptionDetection> DescriptionDetections { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PatientModel>()
                .HasMany(p => p.DescriptionDetections)
                .WithOne(d => d.Patient)
                .HasForeignKey(d => d.PatientId);

        }

    }
}
