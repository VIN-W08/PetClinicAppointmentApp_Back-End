using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PetClinicAppointmentApp.Models
{
    public class ScheduleService
    {
        [Key]
        public int Schedule_service_id { get; set; }
        public int Service_id { get; set; }

        [JsonIgnore]
        public Service Service { get; set; }
        public DateTime Start_schedule { get; set; }
        public DateTime End_schedule { get; set; }
        public bool Status { get; set; } = true;
        public int Quota { get; set; } = 1;
        public DateTime Created_at { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? Updated_at { get; set; }

        [JsonIgnore]
        public bool Is_deleted { get; set; } = false;

        [JsonIgnore]
        public List<Appointment> Appointments { get; set; }
    }
}
