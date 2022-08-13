namespace PetClinicAppointmentApp.Resources
{
    public class CreateClinicScheduleResource
    {
        public int Pet_clinic_id { get;set; }
        public byte Day { get;set; }
        public TimeSpan Start_time { get;set; }
        public TimeSpan End_time { get;set; }
    }
}
