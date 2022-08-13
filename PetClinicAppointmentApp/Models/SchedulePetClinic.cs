using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PetClinicAppointmentApp.Models
{
    public class SchedulePetClinic
    {
        [Key]
        public int Schedule_pet_clinic_id { get; set; }
        public int Pet_clinic_id { get; set; }

        [JsonIgnore]
        public PetClinic Pet_clinic { get; set; }
        public byte Day { get; set; }
        public byte Shift { get; set; }
        public TimeSpan Start_time { get; set; }
        public TimeSpan End_time { get; set; }
        public DateTime Created_at { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? Updated_at { get; set; }
    }
}
