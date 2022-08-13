using PetClinicAppointmentApp.Models;

namespace PetClinicAppointmentApp.Resources
{
    public class ServiceDetailResource
    {
        public int Service_id { get; set; }
        public int Pet_clinic_id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool Status { get; set; } = true;
        public DateTime Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        public List<ScheduleService> Schedules_service { get; set; }
    }
}