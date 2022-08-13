using Microsoft.EntityFrameworkCore;
using PetClinicAppointmentApp.Models;

namespace PetClinicAppointmentApp
{
    public class PetClinicAppointmentDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<PetClinic> PetClinics { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ScheduleService> ServiceSchedules { get; set; }
        public DbSet<SchedulePetClinic> PetClinicSchedules { get; set; }

        public PetClinicAppointmentDbContext(DbContextOptions<PetClinicAppointmentDbContext> options): base(options) 
        {
            
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .ToTable("customer")
                .HasQueryFilter(c => !c.Is_deleted)
                .HasIndex(c => c.Email)
                .IsUnique();
            modelBuilder.Entity<PetClinic>()
                .ToTable("pet_clinic")
                .HasQueryFilter(pc => !pc.Is_deleted)
                .HasIndex(pc => pc.Email)
                .IsUnique();
            modelBuilder.Entity<Appointment>()
                .ToTable("appointment")
                .HasQueryFilter(a => !a.Is_deleted)
                .HasOne(a => a.Customer)
                .WithMany(c => c.Appointments)
                .HasForeignKey(a => a.Customer_id);
            modelBuilder.Entity<Appointment>()
                .ToTable("appointment")
                .HasQueryFilter(a => !a.Is_deleted)
                .HasOne(a => a.Pet_clinic)
                .WithMany(pc => pc.Appointments)
                .HasForeignKey(a => a.Pet_clinic_id);
            modelBuilder.Entity<Appointment>()
               .ToTable("appointment")
               .HasQueryFilter(a => !a.Is_deleted)
               .HasOne(a => a.Service)
               .WithMany(s => s.Appointments)
               .HasForeignKey(a => a.Service_id);
            modelBuilder.Entity<Appointment>()
               .ToTable("appointment")
               .HasQueryFilter(a => !a.Is_deleted)
               .HasOne(a => a.Schedule_service)
               .WithMany(ss => ss.Appointments)
               .HasForeignKey(a => a.Schedule_service_id);
            modelBuilder.Entity<Service>()
                .ToTable("service")
                .HasQueryFilter(s => !s.Is_deleted)
                .HasOne(s => s.Pet_clinic)
                .WithMany(pc => pc.Services)
                .HasForeignKey(s => s.Pet_clinic_id);
            modelBuilder.Entity<ScheduleService>()
                .ToTable("schedule_service")
                .HasQueryFilter(ss => !ss.Is_deleted)
                .HasOne(ss => ss.Service)
                .WithMany(s => s.Schedules_service)
                .HasForeignKey(ss => ss.Service_id);
            modelBuilder.Entity<SchedulePetClinic>()
                .ToTable("schedule_pet_clinic")
                .HasOne(spc => spc.Pet_clinic)
                .WithMany(pc => pc.Schedules_pet_clinic)
                .HasForeignKey(spc => spc.Pet_clinic_id);
        }
    }
}
