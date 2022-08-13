using System.ComponentModel.DataAnnotations;

namespace PetClinicAppointmentApp.Resources
{
    public class CreateAppointmentResource
    {
        [Required]
        public int Customer_id { get; set; }
        [Required]
        public int Pet_clinic_id { get; set; }
        [Required]
        public int Service_id { get; set; }
        [Required]
        public int Schedule_service_id { get; set; }
        [Required]
        public string Service_name { get; set; }
        [Required]
        public decimal Service_price { get; set; }
        [Required]
        public DateTime Start_schedule { get; set; }
        [Required]
        public DateTime End_schedule { get; set; }
    }
}
