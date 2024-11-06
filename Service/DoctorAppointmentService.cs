using DoctorAppointmentSystemAPI.Data;
using DoctorAppointmentSystemAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DoctorAppointmentSystemAPI.Service
{
    public class DoctorAppointmentService
    {
        private readonly DoctorAppointmentSystemDbContext _context;
        private readonly ILogger<DoctorAppointmentService> _logger;

        public DoctorAppointmentService(DoctorAppointmentSystemDbContext context, ILogger<DoctorAppointmentService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<object> GetAvailableTimeSlotsForDoctorAsync(int doctorId, DateTime date, int duration)
        {
            // Fetch the doctor along with their time slots
            var doctor = await _context.Doctors
                .Include(d => d.TimeSlots)
                .FirstOrDefaultAsync(d => d.Id == doctorId);

            if (doctor == null)
            {
                throw new KeyNotFoundException("Doctor not found.");
            }

            // Filter time slots based on the specified date and availability
            var availableSlots = doctor.TimeSlots
                .Where(ts => ts.DateTime.Date == date.Date && !ts.IsBooked)
                .Select(ts => new
                {
                    ts.Id,
                    StartTime = ts.DateTime,
                    EndTime = ts.DateTime.AddMinutes(duration) // Calculate end time based on duration
                })
                .ToList();

            return new
            {
                DoctorName = doctor.Name,
                Specialization = doctor.Specialization,
                AvailableSlots = availableSlots
            };
        }

        public async Task<Appointment> BookAppointmentAsync(AppointmentRequest request)
        {
            var slot = await _context.TimeSlots.FirstOrDefaultAsync(ts => ts.Id == request.TimeSlotId);

            if (slot == null || slot.IsBooked || slot.DateTime <= DateTime.Now)
                throw new ArgumentException("Invalid or unavailable time slot.");

            if (!IsValidEmail(request.PatientEmail))
                throw new ArgumentException("Invalid email format.");

            bool hasOverlappingAppointment = await _context.Appointments
                .AnyAsync(a => a.PatientName == request.PatientName && a.AppointmentTime == slot.DateTime);

            if (hasOverlappingAppointment)
            {
                throw new ArgumentException("An overlapping appointment already exists for this patient.");
            }

            slot.IsBooked = true;

            var appointment = new Appointment
            {
                SlotId = slot.Id,
                PatientName = request.PatientName,
                Purpose = request.Purpose,
                AppointmentTime = slot.DateTime
            };

            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();

            return appointment;
        }

        public async Task CancelAppointmentAsync(int appointmentId)
        {
            
            var appointment = await _context.Appointments
                .Include(a => a.Slot)
                .ThenInclude(s => s.Doctor)
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null)
            {
                throw new KeyNotFoundException("Appointment not found.");
            }

            var timeUntilAppointment = appointment.AppointmentTime - DateTime.Now;
            if (timeUntilAppointment.TotalHours < 24)
            {
                throw new InvalidOperationException("Appointments can only be canceled with a minimum 24-hour notice.");
            }

            
            var slot = appointment.Slot;
            if (slot != null)
            {
                slot.IsBooked = false;
            }

            
            _context.Appointments.Remove(appointment);

            
            await _context.SaveChangesAsync();

            
            NotifyDoctor(appointment.Slot.Doctor, appointment);
        }

        
        private void NotifyDoctor(Doctor doctor, Appointment appointment)
        {
            
            if (doctor != null)
            {
                Console.WriteLine($"Notification: Dr. {doctor.Name} has been notified about the cancellation of the appointment with {appointment.PatientName} scheduled for {appointment.AppointmentTime}.");
            }
        }


        public async Task<IEnumerable<object>> GetDoctorScheduleAsync(DateTime date)
        {
            date = date.ToUniversalTime();
            return await _context.Appointments
                .Where(a => _context.TimeSlots.Any(ts => ts.Id == a.SlotId && ts.DateTime.Date == date.Date))
                .Select(a => new
                {
                    Time = a.AppointmentTime.ToString("hh:mm tt"),
                    a.PatientName,
                    a.Purpose,
                    Status = "Scheduled"
                })
                .ToListAsync();
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

    }
}
