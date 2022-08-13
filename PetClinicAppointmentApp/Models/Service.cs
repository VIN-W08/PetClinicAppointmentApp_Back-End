using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PetClinicAppointmentApp.Models
{
    public class Service
    {
        [Key]
        public int Service_id { get; set; }
        public int Pet_clinic_id { get; set; }
        
        [JsonIgnore]
        public PetClinic Pet_clinic { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool Status { get; set; } = true;

        public DateTime Created_at { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? Updated_at { get; set; }

        [JsonIgnore]
        public bool Is_deleted { get; set; } = false;

        [JsonIgnore]
        public List<ScheduleService> Schedules_service { get; set; }
        [JsonIgnore]
        public List<Appointment> Appointments { get; set; }
    }
}
