using PetClinicAppointmentApp.Models;
using System.Numerics;
using System.Text.Json.Serialization;

namespace PetClinicAppointmentApp.Resources
{
    public class PetClinicDetailResource
    {
        [JsonPropertyName("user_id")]
        public int Pet_clinic_id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [JsonPropertyName("phone_num")]
        public string? Phone_number { get; set; }
        public byte[]? Image { get; set; }
        public string Address { get; set; }
        public long? Village_id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool Status { get; set; }
        public List<Service> Services { get; set; }
        public List<SchedulePetClinic> Schedules_pet_clinic { get; set; }
        public DateTime? Updated_at { get; set; }
        public DateTime Created_at { get; set; }
    }
}
