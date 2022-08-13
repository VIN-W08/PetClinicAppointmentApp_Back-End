using System.Text.Json.Serialization;

namespace PetClinicAppointmentApp.Resources
{
    public class CustomerResource
    {
        [JsonPropertyName("user_id")]
        public int Customer_id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone_num { get; set; }
        public byte[]? Image { get; set; }
        public DateTime? Updated_at { get; set; }
        public DateTime Created_at { get; set; }
    }
}
