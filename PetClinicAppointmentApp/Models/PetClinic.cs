using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PetClinicAppointmentApp.Models
{
    public class PetClinic: User
    {
        [Key]
        public int Pet_clinic_id { get; set; }
        [JsonPropertyName("phone_num")]
        public string Phone_number { get; set; }
        public string? Image_name { get; set; }
        public string Address { get; set; }
        public long Village_id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool Status { get; set; } = true;

        [NotMapped]
        public double? Distance { get; set; } = null;
        
        [JsonIgnore]
        public List<Appointment> Appointments { get; set; }
        
        [JsonIgnore]
        public List<Service> Services { get; set; }

        [JsonIgnore]
        public List<SchedulePetClinic> Schedules_pet_clinic { get; set; }
    }
}
