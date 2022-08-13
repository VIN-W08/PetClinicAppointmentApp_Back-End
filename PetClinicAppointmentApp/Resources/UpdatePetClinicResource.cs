using System.Numerics;
using System.Text.Json.Serialization;

namespace PetClinicAppointmentApp.Resources
{
    public class UpdatePetClinicResource
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        [JsonPropertyName("phone_num")]
        public string? Phone_number { get; set; }
        public string? Address { get; set; }
        public long? Village_id { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public IFormFile? Image { get; set; }
    }
}
