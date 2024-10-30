using DoctorAppointmentSystemAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace DoctorAppointmentSystemAPI.Service
{
    public class DoctorService
    {
        private readonly DoctorAppointmentSystemDbContext _context;
        public DoctorService(DoctorAppointmentSystemDbContext context)
        {
            _context = context;
        }
        public async Task<object> GetAvailableSlots(int doctorId, DateTime date, int duration )
        {
            var doctor = await _context.Doctors
                .Include(d => d.TimeSlots)
                .FirstOrDefaultAsync(d => d.Id == doctorId);

            if (doctor == null)
            {
                throw new KeyNotFoundException("Doctor not found.");
            }

            var availableSlots = doctor.TimeSlots
                .Where(slot => slot.DateTime.Date == date.Date && !slot.IsBooked)
                .Select(slot => new
                {
                    slot.Id,
                    StartTime = slot.DateTime,
                    EndTime = slot.DateTime.AddMinutes(duration)
                })
                .ToList();

            return new
            {
                DoctorName = doctor.Name,
                Specialization = doctor.Specialization,
                AvailableSlots = availableSlots
            };
            
        }

        public async Task<object> GetDoctorSchedule(int doctorId, DateTime date)
        {
            var doctor = await _context.Doctors
                .Include(d => d.TimeSlots)
                .FirstOrDefaultAsync(d => d.Id == doctorId);

            if (doctor == null)
            {
                throw new KeyNotFoundException("Doctor not found.");
            }
            var schedule = doctor.TimeSlots
                .Where(slot => slot.DateTime.Date == date.Date && slot.IsBooked)
                .Select(slot => new
                {
                    Time = slot.DateTime.ToString("hh:mm tt"),
                    PatientName = "Patient Name", // Replace with actual patient name if available
                    Purpose = "Purpose", // Replace with actual purpose if available
                    Status = "Scheduled"
                })
                .ToList();

            return new
            {
                Date = date.ToString("yyyy-MM-dd"),
                Appointments = schedule

            };

        }


    }
}
