using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PetClinicAppointmentApp.Models
{
    public class Appointment
    {
        [Key]
        public int Appointment_id { get; set; }
        public int Customer_id { get; set; }

        [JsonIgnore]
        public Customer Customer { get; set; }
        public int Pet_clinic_id { get; set; }

        [JsonIgnore]
        public PetClinic Pet_clinic { get; set; }
        public int? Service_id { get; set; }

        [JsonIgnore]
        public Service? Service { get; set; }
        public int? Schedule_service_id { get; set; }

        [JsonIgnore]
        public ScheduleService? Schedule_service { get; set; }
        public string Service_name { get; set; }
        public decimal Service_price {  get; set; }
        public DateTime Start_schedule { get; set; }
        public DateTime End_schedule { get; set; }
        public byte Status { get; set; } = 0;
        public DateTime Created_at { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? Updated_at { get; set; }

        [JsonIgnore]
        public bool Is_deleted { get; set; } = false;
    }
}
