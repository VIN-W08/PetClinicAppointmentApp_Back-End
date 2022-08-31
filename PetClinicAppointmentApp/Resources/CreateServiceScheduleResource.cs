

namespace PetClinicAppointmentApp.Resources
{
    public class CreateServiceScheduleResource
    {
        public int Service_id { get; set; }
        public DateTime Start_schedule { get; set; }
        public DateTime End_schedule { get; set; }
        public int Quota { get; set; } = 1;
        public int Repeat_schedule_Week_count { get; set; } = 0;
    }
}
