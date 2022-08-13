using PetClinicAppointmentApp.Models;

namespace PetClinicAppointmentApp.Interfaces
{
    public interface ICustomerService
    {
        Task<Customer?> GetCustomerById(int customerId);
        Task<Customer> CreateCustomer(string name, string email, string password);
        Task<Customer?> UpdateCustomer(int customerId, Customer customer);
        Task<Customer?> ChangePassword(string email, string newPassword);
    }
}
