using PetClinicAppointmentApp.Models;

namespace PetClinicAppointmentApp.Interfaces
{
    public interface IServiceScheduleService
    {
        Task<List<ScheduleService>> GetServiceScheduleList(int? serviceId, DateTime? startSchedule);
        ScheduleService? GetServiceScheduleDetail(int id);
        Task<ScheduleService> CreateServiceSchedule(ScheduleService schedule, int repeatScheduleWeekCount);
        Task<ScheduleService?> UpdateServiceSchedule(int scheduleId, ScheduleService schedule, int? quota, bool? status);
        Task<ScheduleService?> DeleteServiceSchedule(int scheduleId);
        Task<ScheduleService?> AddSubstractServiceScheduleQuota(int scheduleId, int quota = 1);
    }
}
