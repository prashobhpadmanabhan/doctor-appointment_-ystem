namespace DoctorAppointmentSystemAPI.DTOs
{
    public class AppointmentRequest
    {
        public int DoctorId { get; set; }
        public int TimeSlotId { get; set; }
        public string PatientName { get; set; }
        public string PatientEmail { get; set; }
        public string Purpose { get; set; }
    }
}
