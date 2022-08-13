namespace PetClinicAppointmentApp.Resources
{
    public class UpdateClinicScheduleResource
    {
        public Byte? Day { get; set; }
        public Byte? Shift { get; set; }
        public TimeSpan? Start_time { get; set; }
        public TimeSpan? End_time { get; set; }
    }
}
