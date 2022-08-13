using PetClinicAppointmentApp.Models;

namespace PetClinicAppointmentApp.Interfaces
{
    public interface IServiceService
    {
        Task<Service> CreateService(Service service);
        Task<Service?> UpdateService(int serviceId, Service service, bool? status);
        Task<Service?> DeleteService(int serviceId);
        Task<List<Service>> GetServiceList(int? petClinicId, string name = "");
        Task<Service?> GetServiceDetail(int serviceId);
    }
}
