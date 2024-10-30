namespace DoctorAppointmentSystemAPI.Data
{
    public class Appointment
    {
        public int Id { get; set; }
        public int SlotId { get; set; }
        public string PatientName { get; set; }
        public string Purpose { get; set; }

    }
}
