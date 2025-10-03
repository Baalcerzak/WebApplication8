using Microsoft.EntityFrameworkCore;
using WebApplication8.Models;

namespace WebApplication8.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Prescription_Medicament> PMs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Patient>().HasKey(c => c.IdPatient);
            modelBuilder.Entity<Doctor>().HasKey(d => d.IdDoctor);
            modelBuilder.Entity<Medicament>().HasKey(p => p.IdMedicament);
            modelBuilder.Entity<Prescription>().HasKey(s => s.IdPrescription);
            modelBuilder.Entity<Prescription_Medicament>().HasKey(s => s.IdPrescription);
            modelBuilder.Entity<Prescription_Medicament>().HasKey(s => s.IdMedicament);

            
            modelBuilder.Entity<Prescription>()
                .HasOne(s => s.Doctor)
                .WithMany(c => c.Prescriptions)
                .HasForeignKey(s => s.IdDoctor);

            modelBuilder.Entity<Prescription>()
                .HasOne(s => s.Patient)
                .WithMany(sub => sub.Prescriptions)
                .HasForeignKey(s => s.IdPatient);

            modelBuilder.Entity<Prescription_Medicament>()
                .HasOne(p => p.Prescription)
                .WithMany(c => c.PMs)
                .HasForeignKey(p => p.IdPrescription);

            modelBuilder.Entity<Prescription_Medicament>()
                .HasOne(p => p.Medicament)
                .WithMany(sub => sub.PMs)
                .HasForeignKey(p => p.IdMedicament);
            
        }
    }
}