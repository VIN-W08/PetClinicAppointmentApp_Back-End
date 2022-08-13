using PetClinicAppointmentApp.Models;

namespace PetClinicAppointmentApp.Interfaces
{
    public interface IAuthService
    {
        Task<Customer?> AuthCustomer(string email, string password);
        Task<PetClinic?> AuthPetClinic(string email, string password);
    }
}
