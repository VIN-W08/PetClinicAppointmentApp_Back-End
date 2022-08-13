using System.ComponentModel.DataAnnotations;

namespace PetClinicAppointmentApp.Resources
{
    public class RegisterCustomerResource
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set;}
    }
}
