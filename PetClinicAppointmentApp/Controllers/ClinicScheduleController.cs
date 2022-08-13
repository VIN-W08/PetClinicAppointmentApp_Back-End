using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PetClinicAppointmentApp.Interfaces;
using PetClinicAppointmentApp.Models;
using PetClinicAppointmentApp.Resources;
using System.Net;

namespace PetClinicAppointmentApp.Controllers
{
    [Route("api/clinic/schedule")]
    [ApiController]
    public class ClinicScheduleController : ControllerBase
    {
        private readonly IClinicScheduleService clinicScheduleService;
        private readonly IMapper mapper;
        public ClinicScheduleController(IClinicScheduleService clinicScheduleService, IMapper mapper)
        {
            this.clinicScheduleService = clinicScheduleService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("list")]
        public async Task<ActionResult<ResponseResource>> Get([FromQuery]int? clinic_id, [FromQuery] int? day)
        {
            var scheduleList = await clinicScheduleService.GetClinicScheduleList(clinic_id, day);
            var responseResource = new ResponseResource
            {
                Status = new RequestStatus
                {
                    Code = HttpStatusCode.OK,
                    Message = "Berhasil"
                },
                Data = scheduleList
            };
            return Ok(responseResource);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ResponseResource>> GetDetail(int id)
        {
            var schedule = await clinicScheduleService.GetClinicScheduleDetail(id);
            if(schedule == null)
            {
                return Ok(new ResponseResource
                {
                    Status = new RequestStatus
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Jadwal operasi klinik tidak ditemukan"
                    }
                });
            }
            var responseResource = new ResponseResource
            {
                Status = new RequestStatus
                {
                    Code = HttpStatusCode.OK,
                    Message = "Berhasil"
                },
                Data = schedule
            };
            return Ok(responseResource);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseResource>> Create([FromBody] CreateClinicScheduleResource createClinicScheduleResource)
        {
            if (createClinicScheduleResource.Start_time >= createClinicScheduleResource.End_time)
            {
                return Ok(new ResponseResource
                {
                    Status = new RequestStatus
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Waktu mulai harus lebih awal dari waktu berakhir"
                    }
                });
            };
            var schedule = mapper.Map<CreateClinicScheduleResource, SchedulePetClinic>(createClinicScheduleResource);
            var createdSchedule = await clinicScheduleService.CreateClinicSchedule(schedule);
            var responseResource = new ResponseResource
            {
                Status = new RequestStatus
                {
                    Code = HttpStatusCode.Created,
                    Message = "Berhasil"
                },
                Data = createdSchedule
            };
            return Created("", responseResource);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ResponseResource>> Update(int id, [FromBody] UpdateClinicScheduleResource updateClinicScheduleResource)
        {
            if (updateClinicScheduleResource.Start_time >= updateClinicScheduleResource.End_time)
            {
                return Ok(new ResponseResource
                {
                    Status = new RequestStatus
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Waktu mulai harus lebih awal dari waktu berakhir"
                    }
                });
            };
            var schedule = mapper.Map<UpdateClinicScheduleResource, SchedulePetClinic>(updateClinicScheduleResource);
            var updatedSchedule = await clinicScheduleService.UpdateClinicSchedule(id, schedule);
            if (updatedSchedule == null)
            {
                return Ok(new ResponseResource
                {
                    Status = new RequestStatus
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Jadwal tidak ditemukan"
                    }
                });
            }
            var responseResource = new ResponseResource
            {
                Status = new RequestStatus
                {
                    Code = HttpStatusCode.OK,
                    Message = "Berhasil"
                },
                Data = updatedSchedule
            };
            return Created("", responseResource);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ResponseResource>> Delete(int id)
        {
            var deletedSchedule = await clinicScheduleService.DeleteClinicScheduleList(id);
            if (deletedSchedule == null)
            {
                return Ok(new ResponseResource
                {
                    Status = new RequestStatus
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Jadwal tidak ditemukan"
                    }
                });
            }
            var responseResource = new ResponseResource
            {
                Status = new RequestStatus
                {
                    Code = HttpStatusCode.OK,
                    Message = "Berhasil"
                },
                Data = deletedSchedule
            };
            return Ok(responseResource);
        }
    }
}
