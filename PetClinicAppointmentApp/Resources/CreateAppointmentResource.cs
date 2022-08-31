

namespace PetClinicAppointmentApp.Resources
{
    public class CreateAppointmentResource
    {
        public int Customer_id { get; set; }
        public int Pet_clinic_id { get; set; }
        public int Service_id { get; set; }
        public int Schedule_service_id { get; set; }
        public string Service_name { get; set; }
        public decimal Service_price { get; set; }
        public DateTime Start_schedule { get; set; }
        public DateTime End_schedule { get; set; }
    }
}
