
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PetClinicAppointmentApp.Models
{
    public abstract class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? Updated_at { get; set; }
        public DateTime Created_at { get; set; } = DateTime.Now;

        [JsonIgnore]
        public bool Is_deleted { get; set; } = false;
    }
}
