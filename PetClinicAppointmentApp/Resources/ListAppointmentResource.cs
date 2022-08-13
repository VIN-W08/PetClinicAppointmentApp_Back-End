using PetClinicAppointmentApp.Models;

namespace PetClinicAppointmentApp.Resources
{
    public class ListAppointmentResource
    {
        public int Appointment_id { get; set; }
        public CustomerResource Customer { get; set; }
        public PetClinicResource Pet_clinic { get; set; }
        public int Service_id { get; set; }
        public int Schedule_service_id { get; set; }
        public decimal Total_payable { get; set; }
        public string Note { get; set; }
        public Byte Status { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
    }
}
