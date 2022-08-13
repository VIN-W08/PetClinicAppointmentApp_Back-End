using Microsoft.EntityFrameworkCore;
using PetClinicAppointmentApp.Interfaces;
using PetClinicAppointmentApp.Models;

namespace PetClinicAppointmentApp.Services
{
    public class CustomerService: ICustomerService
    {
        private readonly PetClinicAppointmentDbContext petClinicAppointmentDbContext;

        public CustomerService(PetClinicAppointmentDbContext petClinicAppointmentDbContext)
        {
            this.petClinicAppointmentDbContext = petClinicAppointmentDbContext;
        }

        public async Task<bool> IsEmailExist(int? id, string email)
        {
            return (
                await petClinicAppointmentDbContext.Customers.AnyAsync(customer => customer.Customer_id != id ? customer.Email == email : false)
               );
        }

        public async Task<Customer?> GetCustomerById(int customerId)
        {
            var customer = await petClinicAppointmentDbContext.Customers.SingleOrDefaultAsync(customer => customer.Customer_id == customerId);
            if(customer == null)
            {
                return null;
            }
            return customer;
        }

        public async Task<Customer> CreateCustomer(string name, string email, string password)
        {
            if (await IsEmailExist(null, email))
            {
                throw new ArgumentException();
            }
            var customer = new Customer {
                Name = name,
                Email = email,
                Password = BCrypt.Net.BCrypt.HashPassword(password)
            };
            petClinicAppointmentDbContext.Customers.Add(customer);
            await petClinicAppointmentDbContext.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer?> UpdateCustomer(
            int customerId, 
            Customer customer
        )
        {
            var selectedCustomer = await petClinicAppointmentDbContext.Customers.FindAsync(customerId);
            if (selectedCustomer == null)
            {
                return null;
            }
            if (await IsEmailExist(customerId, customer.Email))
            {
                throw new ArgumentException();
            }

            if (!string.IsNullOrEmpty(customer.Name))
            {
                selectedCustomer.Name = customer.Name;
            }
            if (!string.IsNullOrEmpty(customer.Email))
            {
                selectedCustomer.Email = customer.Email;
            }

            await petClinicAppointmentDbContext.SaveChangesAsync();
            return selectedCustomer;
        }

        public async Task<Customer?> ChangePassword(string email, string newPassword)
        {
            var selectedCustomer = await petClinicAppointmentDbContext.Customers.SingleOrDefaultAsync(customer => customer.Email.Equals(email));
            if(selectedCustomer == null)
            {
                return null;
            }
            selectedCustomer.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await petClinicAppointmentDbContext.SaveChangesAsync();
            return selectedCustomer;
        }
    }
}
