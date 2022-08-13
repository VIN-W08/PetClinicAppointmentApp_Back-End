using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PetClinicAppointmentApp.Models
{
    public class Customer: User
    {
        [Key]
        public int Customer_id { get; set; }
        
        [JsonIgnore]
        public List<Appointment> Appointments { get; set; }
    }
}
