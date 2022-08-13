using System.Text.Json.Serialization;

namespace PetClinicAppointmentApp.Models
{
    public abstract class AuthResource
    {
        public Boolean Status { get; set; }

        public string Role { get; set; }

        public DateTime Created_at { get; set; }
    }
}
