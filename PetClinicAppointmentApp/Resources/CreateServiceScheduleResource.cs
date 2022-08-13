using System.ComponentModel.DataAnnotations;

namespace PetClinicAppointmentApp.Resources
{
    public class CreateServiceScheduleResource
    {
        [Required]
        public int Service_id { get; set; }
        [Required]
        public DateTime Start_schedule { get; set; }
        [Required]
        public DateTime End_schedule { get; set; }
        [Required]
        public int Quota { get; set; } = 1;
        public int Repeat_schedule_Week_count { get; set; } = 0;
    }
}
