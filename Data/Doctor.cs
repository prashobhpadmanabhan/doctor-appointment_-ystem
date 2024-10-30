namespace DoctorAppointmentSystemAPI.Data
{
    public class Doctor
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }
        public ICollection<TimeSlot> TimeSlots { get; set; }
    }
}
