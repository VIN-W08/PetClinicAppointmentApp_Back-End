using Microsoft.EntityFrameworkCore;
using PetClinicAppointmentApp.Interfaces;
using PetClinicAppointmentApp.Models;

namespace PetClinicAppointmentApp.Services
{
    public class ServiceService: IServiceService
    {
        private readonly PetClinicAppointmentDbContext petClinicAppointmentDbContext;
        
        public ServiceService(PetClinicAppointmentDbContext petClinicAppointmentDbContext)
        {
            this.petClinicAppointmentDbContext = petClinicAppointmentDbContext;
        }

        public byte CheckAppointmentStatus(Appointment appointment)
        {
            if (
                (appointment.Status.Equals(0)) &&
                (appointment.Start_schedule <= DateTime.Now)
            )
            {
                return 3;
            }
            return appointment.Status;
        }

        public async Task<Service> CreateService(Service service)
        {
            petClinicAppointmentDbContext.Services.Add(service);
            await petClinicAppointmentDbContext.SaveChangesAsync();
            return service;
        }
        public async Task<Service?> UpdateService(int serviceId, Service service, bool? status)
        {
            var selectedService = await petClinicAppointmentDbContext.Services
                .Where(service => service.Service_id == serviceId)
                .Include(service => service.Appointments)
                .FirstOrDefaultAsync();
            
            if (selectedService == null)
            {
                return null;
            }

            selectedService.Appointments.ForEach(appointment => appointment.Status = CheckAppointmentStatus(appointment));
            if(selectedService.Appointments.Exists(appointment => appointment.Status == 0 || appointment.Status == 1))
            {
                throw new InvalidOperationException();
            }

            if (!string.IsNullOrEmpty(service.Name))
            {
                selectedService.Name = service.Name;
            }
            if (service.Price != 0)
            {
                selectedService.Price = service.Price;
            }
            if(status != null)
            {
                selectedService.Status = (bool)status;
            }
            await petClinicAppointmentDbContext.SaveChangesAsync();
            return selectedService;
        }
        public async Task<Service?> DeleteService(int serviceId)
        {
            var selectedService = await petClinicAppointmentDbContext.Services.FindAsync(serviceId);
            if(selectedService == null)
            {
                return null;
            }
            selectedService.Is_deleted = true;
            await petClinicAppointmentDbContext.SaveChangesAsync();
            return selectedService;
        }

        public async Task<List<Service>> GetServiceList(int? petClinicId, string name)
        {
            var serviceList = await petClinicAppointmentDbContext.Services
               .Where(service =>
               (petClinicId == null || service.Pet_clinic_id == petClinicId) &&
               (service.Name.ToLower().Contains(name.ToLower())))
               .ToListAsync();
          
            return serviceList;
        }
         
        public async Task<Service?> GetServiceDetail(int serviceId)
        {
            var selectedService = await petClinicAppointmentDbContext.Services
               .Where(service => service.Service_id == serviceId)
               .Include(service => service.Schedules_service)
               .FirstOrDefaultAsync();
            return selectedService;
        }
    }
}
