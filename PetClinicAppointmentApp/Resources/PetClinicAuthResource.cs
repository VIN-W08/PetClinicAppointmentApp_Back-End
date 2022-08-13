using PetClinicAppointmentApp.Models;

namespace PetClinicAppointmentApp.Resources
{
    public class PetClinicAuthResource: AuthResource
    {
        public AuthPetClinicResource Pet_clinic { get; set; }
    }
}
