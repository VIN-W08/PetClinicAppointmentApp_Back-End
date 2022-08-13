namespace PetClinicAppointmentApp.Resources
{
    public class UpdateServiceScheduleResource
    {
        public DateTime? Start_schedule { get; set; }
        public DateTime? End_schedule { get; set; }
        public int? Quota { get; set; }
        public bool? Status { get; set; }
    }
}
