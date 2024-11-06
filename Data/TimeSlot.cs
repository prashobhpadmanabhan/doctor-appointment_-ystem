namespace DoctorAppointmentSystemAPI.Data
{
    public class TimeSlot
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        private DateTime _dateTime;
        public DateTime DateTime
        {
            get => _dateTime;
            set => _dateTime = DateTime.SpecifyKind(value, DateTimeKind.Utc); 
        }

        public bool IsBooked { get; set; }
        
    }
}
