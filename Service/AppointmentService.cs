using DoctorAppointmentSystemAPI.Data;
using DoctorAppointmentSystemAPI.DTOs;

namespace DoctorAppointmentSystemAPI.Service
{
    public class AppointmentService
    {
        private readonly DoctorAppointmentSystemDbContext _context;

        public AppointmentService(DoctorAppointmentSystemDbContext context)
        {
            _context = context; 
        }

        public async Task<Appointment> BookAppointment(AppointmentRequest request)
        {
            var slot = await _context.TimeSlots.FindAsync(request.TimeSlotId);
            if (slot == null || slot.IsBooked || slot.DateTime <= DateTime.Now)
            {
                throw new InvalidOperationException("unavailable time slot.");
            }

            var appointment = new Appointment
            {
                SlotId = request.TimeSlotId,
                PatientName = request.PatientName,
                Purpose = request.Purpose
            };

            slot.IsBooked = true;
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return appointment;
        }

        public async Task CancelAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                throw new KeyNotFoundException("Appointment not found.");
            }

            var slot = await _context.TimeSlots.FindAsync(appointment.SlotId);
            if (slot != null)
            {
                slot.IsBooked = false;
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
        }

    }
}
