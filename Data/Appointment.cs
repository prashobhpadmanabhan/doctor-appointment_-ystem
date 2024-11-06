namespace DoctorAppointmentSystemAPI.Data
{
    public class Appointment
    {
        public int Id { get; set; }
        public int SlotId { get; set; }
        public string PatientName { get; set; }

        public DateTime AppointmentTime { get; set; }
        public string Purpose { get; set; }
        public TimeSlot Slot { get; set; }

    }
}
