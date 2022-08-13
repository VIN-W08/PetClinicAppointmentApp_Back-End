using PetClinicAppointmentApp.Resources;
using System.Text.Json.Serialization;

namespace PetClinicAppointmentApp.Models
{
    public class CustomerAuthResource: AuthResource
    {
        public AuthCustomerResource Customer { get; set; }
    }
}
