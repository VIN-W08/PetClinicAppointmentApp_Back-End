using Microsoft.EntityFrameworkCore;
using PetClinicAppointmentApp.Interfaces;
using PetClinicAppointmentApp.Models;

namespace PetClinicAppointmentApp.Services
{
    public class ClinicScheduleService: IClinicScheduleService
    {
        private readonly PetClinicAppointmentDbContext petClinicAppointmentDbContext;
        public ClinicScheduleService(PetClinicAppointmentDbContext petClinicAppointmentDbContext)
        {
            this.petClinicAppointmentDbContext = petClinicAppointmentDbContext;
        }

        public async Task<List<SchedulePetClinic>> GetClinicScheduleList(int? clinicId, int? day)
        {
            var scheduleList = await petClinicAppointmentDbContext.PetClinicSchedules
                .Where(schedule =>
                    (clinicId == null || schedule.Pet_clinic_id == clinicId) &&
                    (day == null || schedule.Day == day)
                 )
                .OrderBy(schedule => schedule.Start_time)
                .ToListAsync();
            return scheduleList;
        }

        public async Task<SchedulePetClinic?> GetClinicScheduleDetail(int scheduleId)
        {
            var schedule = await petClinicAppointmentDbContext.PetClinicSchedules.FindAsync(scheduleId);
            if(schedule == null)
            {
                return null;
            }
            return schedule;
        }

        public async Task<SchedulePetClinic> CreateClinicSchedule(SchedulePetClinic schedule)
        {
            var sameDayScheduleList = await petClinicAppointmentDbContext.PetClinicSchedules
                .Where(s => s.Pet_clinic_id == schedule.Pet_clinic_id && s.Day == schedule.Day)
                .ToListAsync();
            schedule.Shift = (byte)(sameDayScheduleList.Count + 1);

            petClinicAppointmentDbContext.PetClinicSchedules.Add(schedule);
            await petClinicAppointmentDbContext.SaveChangesAsync();
            return schedule;
        }
        public async Task<SchedulePetClinic> UpdateClinicSchedule(int scheduleId, SchedulePetClinic schedule)
        {
            var selectedSchedule = await petClinicAppointmentDbContext.PetClinicSchedules.FindAsync(scheduleId);
            if (selectedSchedule == null)
            {
                return null;
            }
            if(schedule.Day != 0)
            {
                selectedSchedule.Day = schedule.Day;
            }
            if(schedule.Shift != 0)
            {
                selectedSchedule.Shift = schedule.Shift;
            }
            if (schedule.Start_time != new TimeSpan(0, 0, 0))
            {
                selectedSchedule.Start_time = schedule.Start_time;
            }
            if (schedule.End_time != new TimeSpan(0, 0, 0))
            {
                selectedSchedule.End_time = schedule.End_time;
            }
            await petClinicAppointmentDbContext.SaveChangesAsync();
            return selectedSchedule;
        }
        public async Task<SchedulePetClinic> DeleteClinicScheduleList(int scheduleId)
        {
            var selectedSchedule = await petClinicAppointmentDbContext.PetClinicSchedules.FindAsync(scheduleId);
            if (selectedSchedule == null)
            {
                return null;
            }
            petClinicAppointmentDbContext.PetClinicSchedules.Remove(selectedSchedule);
            await petClinicAppointmentDbContext.SaveChangesAsync();
            return selectedSchedule;
        }
    }
}
