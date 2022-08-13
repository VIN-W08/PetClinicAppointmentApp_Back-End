using PetClinicAppointmentApp.Models;

namespace PetClinicAppointmentApp.Interfaces
{
    public interface IAppointmentService
    {
        public Task<List<Appointment>> GetAppointmentList(int? clinicId, int? customerId, byte? status, DateTime? startSchedule, DateTime? fromSchedule, bool? finished, string? sortOrder, int pageNumber, int pageSize);
        public Task<Appointment?> GetAppointmentDetail(int id);
        public Task<Appointment> CreateAppointment(Appointment appointment);
        public Task<Appointment?> UpdateAppointmentStatus(int appointmentId, byte status);
    }
}
