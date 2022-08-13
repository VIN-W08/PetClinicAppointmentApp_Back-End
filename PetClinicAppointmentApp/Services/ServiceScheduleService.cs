using Microsoft.EntityFrameworkCore;
using PetClinicAppointmentApp.Interfaces;
using PetClinicAppointmentApp.Models;

namespace PetClinicAppointmentApp.Services
{
    public class ServiceScheduleService: IServiceScheduleService
    {
        public readonly PetClinicAppointmentDbContext petClinicAppointmentDbContext;

        public ServiceScheduleService(PetClinicAppointmentDbContext petClinicAppointmentDbContext)
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
            else if (
               (appointment.Status.Equals(1)) &&
               (appointment.Start_schedule.AddMinutes(30) <= DateTime.Now)
           )
            {
                return 3;
            }
            return appointment.Status;
        }
        public async Task<List<ScheduleService>> GetServiceScheduleList(int? serviceId, DateTime? startSchedule)
        {
            var scheduleList = await petClinicAppointmentDbContext.ServiceSchedules
                .Where(schedule =>
                    (serviceId == null || schedule.Service_id == serviceId) &&
                    (startSchedule == null || schedule.Start_schedule.Date == startSchedule.Value.Date)
                )
                .OrderBy(schedule => schedule.Start_schedule)
                .ToListAsync();
            return scheduleList;
        }

        public ScheduleService? GetServiceScheduleDetail(int id)
        {
            var selectedScheduleService = petClinicAppointmentDbContext.ServiceSchedules
                .Where(serviceSchedule => serviceSchedule.Schedule_service_id.Equals(id))
                .Include(serviceSchedule => serviceSchedule.Service)
                .FirstOrDefault();
            return selectedScheduleService;
        }

        public async Task<ScheduleService> CreateServiceSchedule(ScheduleService schedule, int repeatScheduleWeekCount)
        {
            var addScheduleList = new List<ScheduleService>();
            var week = 0;

            do
            { 
                var addStartSchedule = schedule.Start_schedule.AddDays(week * 7);
                var addEndSchedule = schedule.End_schedule.AddDays(week * 7);
                var addSchedule = new ScheduleService   
                {
                    Service_id = schedule.Service_id,
                    Start_schedule = addStartSchedule,
                    End_schedule = addEndSchedule,
                    Quota = schedule.Quota
                };
                addScheduleList.Add(addSchedule);
                week++;
            } while (week <= repeatScheduleWeekCount);
            petClinicAppointmentDbContext.ServiceSchedules.AddRange(addScheduleList);
            await petClinicAppointmentDbContext.SaveChangesAsync();
            
            return schedule;
        }

        public async Task<ScheduleService?> UpdateServiceSchedule(int scheduleId, ScheduleService schedule, int? quota, bool? status)
        {
            var selectedSchedule = await petClinicAppointmentDbContext.ServiceSchedules
                .Where(schedule => schedule.Schedule_service_id == scheduleId)
                .Include(schedule => schedule.Appointments)
                .FirstOrDefaultAsync();
            if (selectedSchedule == null)
            {
                return null;
            }

            selectedSchedule.Appointments.ForEach(appointment => appointment.Status = CheckAppointmentStatus(appointment));
            if (selectedSchedule.Appointments.Exists(appointment => appointment.Status == 0 || appointment.Status == 1))
            {
                throw new InvalidOperationException();
            }

            if (schedule.Start_schedule != DateTime.MinValue)
            {
                selectedSchedule.Start_schedule = schedule.Start_schedule;
            }
            if (schedule.End_schedule != DateTime.MinValue)
            {
                selectedSchedule.End_schedule = schedule.End_schedule;
            }
            if(quota != null)
            {
                selectedSchedule.Quota = schedule.Quota;
            }
            if(status != null)
            {
                selectedSchedule.Status = schedule.Status;
            }
            await petClinicAppointmentDbContext.SaveChangesAsync();
            return selectedSchedule;
        }

        public async Task<ScheduleService?> DeleteServiceSchedule(int scheduleId)
        {
            var selectedSchedule = await petClinicAppointmentDbContext.ServiceSchedules.FindAsync(scheduleId);
            if (selectedSchedule == null)
            {
                return null;
            }
            selectedSchedule.Is_deleted = true;
            await petClinicAppointmentDbContext.SaveChangesAsync();

            return selectedSchedule;
        } 

        public async Task<ScheduleService?> AddSubstractServiceScheduleQuota(int scheduleId, int quota = 1)
        {
            var selectedSchedule = await petClinicAppointmentDbContext.ServiceSchedules
                .IgnoreQueryFilters()
                .Where(schedule => schedule.Schedule_service_id == scheduleId)
                .SingleOrDefaultAsync();
            if (selectedSchedule == null)
            {
                return null;
            }
            selectedSchedule.Quota += quota;
            await petClinicAppointmentDbContext.SaveChangesAsync();
            return selectedSchedule;
        } 
    }
}
