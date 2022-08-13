using PetClinicAppointmentApp.Models;

namespace PetClinicAppointmentApp.Resources
{
    public class AppointmentResource
    {
        public int Appointment_id { get; set; }
        public int Customer_id { get; set; }
        public CustomerResource Customer { get; set; }
        public PetClinicResource Pet_clinic { get; set; }
        public Service Service { get; set; }
        public ScheduleService Schedule_service { get; set; }
        public string Service_name { get; set; }
        public decimal Service_price { get; set; }
        public DateTime Start_schedule { get; set; }
        public DateTime End_schedule { get; set; }
        public decimal Total_payable { get; set; }
        public string Note { get; set; }
        public Byte Status { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
    }
}
