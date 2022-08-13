using PetClinicAppointmentApp.Models;

namespace PetClinicAppointmentApp.Interfaces
{
    public interface IClinicScheduleService
    {
        public Task<List<SchedulePetClinic>> GetClinicScheduleList(int? clinicId, int? day);
        public Task<SchedulePetClinic?> GetClinicScheduleDetail(int scheduleId);
        public Task<SchedulePetClinic> CreateClinicSchedule(SchedulePetClinic schedule);
        public Task<SchedulePetClinic> UpdateClinicSchedule(int scheduleId, SchedulePetClinic schedule);
        public Task<SchedulePetClinic> DeleteClinicScheduleList(int scheduleId);
    }
}
