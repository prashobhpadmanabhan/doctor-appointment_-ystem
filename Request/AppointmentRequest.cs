using System.ComponentModel.DataAnnotations;

namespace DoctorAppointmentSystemAPI.DTOs
{
    public class AppointmentRequest
    {
        [Required]
        public int DoctorId { get; set; }

        [Required]
        public int TimeSlotId { get; set; }
        [Required]
        public string PatientName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string PatientEmail { get; set; }

        [Required]
        public string Purpose { get; set; }
    }
}
