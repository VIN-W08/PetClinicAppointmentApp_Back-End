using Microsoft.EntityFrameworkCore;
using PetClinicAppointmentApp.Interfaces;
using PetClinicAppointmentApp.Models;
using System.Linq;

namespace PetClinicAppointmentApp.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly PetClinicAppointmentDbContext petClinicAppointmentDbContext;

        public AppointmentService(PetClinicAppointmentDbContext petClinicAppointmentDbContext)
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

        public async Task<List<Appointment>> GetAppointmentList(
            int? clinicId, 
            int? customerId, 
            byte? status, 
            DateTime? startSchedule, 
            DateTime? fromSchedule,
            bool? finished, 
            string? sortOrder,
            int pageNumber, 
            int pageSize
        )
        {
            var appointmentList = await petClinicAppointmentDbContext.Appointments
                .IgnoreQueryFilters()
                .Where(appointment =>
                    (clinicId == null || appointment.Pet_clinic_id == clinicId) &&
                    (customerId == null || appointment.Customer_id == customerId) &&
                    (startSchedule == null || appointment.Start_schedule.Date == startSchedule.Value.Date) &&
                    (fromSchedule == null || appointment.Start_schedule.Date >= fromSchedule.Value.Date)
                 )
                .Include(appointment => appointment.Customer)
                .Include(appointment => appointment.Pet_clinic)
                .Include(appointment => appointment.Service)
                .Include(appointment => appointment.Schedule_service)
                .ToListAsync();
            if(sortOrder == "asc-created_at")
            {
                appointmentList = appointmentList
                    .OrderBy(appointment => appointment.Created_at)
                    .ToList();
            }
            else if(sortOrder == "desc-created_at")
            {
                appointmentList = appointmentList
                    .OrderByDescending(appointment => appointment.Created_at)
                    .ToList();
            }else if (sortOrder == "asc-updated_at")
            {
                appointmentList = appointmentList
                    .OrderBy(appointment => appointment.Updated_at)
                    .ToList();
            }
            else if (sortOrder == "desc-updated_at")
            {
                appointmentList = appointmentList
                    .OrderByDescending(appointment => appointment.Updated_at)
                    .ToList();
            }else if (sortOrder == "asc-start_schedule")
            {
                appointmentList = appointmentList
                    .OrderBy(appointment => appointment.Start_schedule)
                    .ThenBy(appointment => appointment.Updated_at)
                    .ToList();
            }
            else if (sortOrder == "desc-start_schedule")
            {
                appointmentList = appointmentList
                    .OrderByDescending(appointment => appointment.Start_schedule)
                    .ToList();
            }
            appointmentList.ForEach(appointment =>
                    appointment.Status = CheckAppointmentStatus(appointment)
                );
            appointmentList = appointmentList.Where(appointment => (status == null || appointment.Status == status))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            if (finished != null)
            {
                if ((bool)finished) 
                {
                    appointmentList = appointmentList.FindAll
                        (appointment =>
                            appointment.Status.Equals(2) ||
                            appointment.Status.Equals(3) ||
                            appointment.Status.Equals(4)
                        );
                }
                else
                {
                    appointmentList = appointmentList.FindAll(appointment =>
                        (appointment.Status.Equals(0) || appointment.Status.Equals(1))
                    );
               
                }
            }
            return appointmentList;
        }

        public async Task<Appointment?> GetAppointmentDetail(int id)
        {
            var selectedAppointment = await petClinicAppointmentDbContext.Appointments
                .Where(appointment => appointment.Appointment_id == id)
                .Include(appointment => appointment.Pet_clinic)
                .Include(appointment => appointment.Customer)
                .Include(appointment => appointment.Service)
                .Include(appointment => appointment.Schedule_service)
                .FirstOrDefaultAsync();
            if (selectedAppointment == null)
            {
                return null;
            }
            selectedAppointment.Status = CheckAppointmentStatus(selectedAppointment);
            return selectedAppointment;
        }

        public async Task<Appointment> CreateAppointment(Appointment appointment)
        {
            petClinicAppointmentDbContext.Appointments.Add(appointment);
            await petClinicAppointmentDbContext.SaveChangesAsync();     
            return appointment;
        }
        public async Task<Appointment?> UpdateAppointmentStatus(int appointmentId, byte status)
        {
            var selectedAppointment = petClinicAppointmentDbContext.Appointments
                .Where(appointment => appointment.Appointment_id.Equals(appointmentId))
                .Include(appointment => appointment.Customer)
                .Include(appointment => appointment.Pet_clinic)
                .Include(appointment => appointment.Service)
                .Include(appointment => appointment.Schedule_service)
                .FirstOrDefault();
            if (selectedAppointment == null)
            {
                return null;
            }

            selectedAppointment.Status = status;
            selectedAppointment.Status = CheckAppointmentStatus(selectedAppointment);

            var finishedStatus = new List<int>() { 2, 3, 4 };
            if (finishedStatus.Contains(selectedAppointment.Status))
            {
                selectedAppointment.Service_id = null;
                selectedAppointment.Schedule_service_id = null;
            }

            await petClinicAppointmentDbContext.SaveChangesAsync();
            return selectedAppointment;
        }
    }
}
