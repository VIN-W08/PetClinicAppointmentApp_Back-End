using PetClinicAppointmentApp.Interfaces;
using PetClinicAppointmentApp.Models;
using Microsoft.EntityFrameworkCore;

namespace PetClinicAppointmentApp.Services
{
    public class AuthService: IAuthService
    {
        private readonly PetClinicAppointmentDbContext petClinicAppointmentDbContext;

        public AuthService(PetClinicAppointmentDbContext petClinicAppointmentDbContext)
        {
            this.petClinicAppointmentDbContext = petClinicAppointmentDbContext;
        }

        public async Task<Customer?> AuthCustomer(string email, string password)
        {
            var customer = await petClinicAppointmentDbContext.Customers.SingleOrDefaultAsync(customer => customer.Email.Equals(email));
            if (customer == null)
            {
                return null;
            }
            var verified = BCrypt.Net.BCrypt.Verify(password, customer.Password);
            if (!verified)
            {
                return null;
            }
            return customer;
        }

        public async Task<PetClinic?> AuthPetClinic(string email, string password)
        {
            var clinic = await petClinicAppointmentDbContext.PetClinics.SingleOrDefaultAsync(clinic => clinic.Email.Equals(email));
            if (clinic == null)
            {
                return null;
            }
            var verified = BCrypt.Net.BCrypt.Verify(password, clinic.Password);
            if (!verified)
            {
                return null;
            }
            return clinic;
        }
    }
}
