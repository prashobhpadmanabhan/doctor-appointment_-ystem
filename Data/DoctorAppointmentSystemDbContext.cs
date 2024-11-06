using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace DoctorAppointmentSystemAPI.Data
{
    public class DoctorAppointmentSystemDbContext : DbContext
    {
        public DoctorAppointmentSystemDbContext(DbContextOptions<DoctorAppointmentSystemDbContext> options) : base(options)
        {

        }
        public DbSet<TimeSlot> TimeSlots { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
            modelBuilder.Entity<TimeSlot>()
                .HasOne(ts => ts.Doctor)
                .WithMany(d => d.TimeSlots)
                .HasForeignKey(ts => ts.DoctorId);

            
        }

    }
}
