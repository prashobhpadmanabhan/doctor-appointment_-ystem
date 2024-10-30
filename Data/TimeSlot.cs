namespace DoctorAppointmentSystemAPI.Data
{
    public class TimeSlot
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsBooked { get; set; }
        
    }
}
