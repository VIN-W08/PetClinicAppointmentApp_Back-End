using System.Text.Json.Serialization;

namespace PetClinicAppointmentApp.Resources
{
    public class CreateAuthResource
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
