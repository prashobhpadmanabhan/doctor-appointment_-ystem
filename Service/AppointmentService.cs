using DoctorAppointmentSystemAPI.Data;
using DoctorAppointmentSystemAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;

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

            if (!IsValidEmail(request.PatientEmail))
            {
                throw new InvalidOperationException("Invalid email format");
            }

            var overlappingAppointment = await _context.Appointments
                .AnyAsync(a=>a.SlotId == request.TimeSlotId && a.PatientName == request.PatientName);
            if (overlappingAppointment)
            {
                throw new InvalidOperationException("The slot is already booked");
            }

            var appointment = new Appointment
            {
                SlotId = request.TimeSlotId,
                PatientName = request.PatientName,
                PatientEmail = request.PatientEmail,
                Purpose = request.Purpose
            };

            slot.IsBooked = true;
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return appointment;
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
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
