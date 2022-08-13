using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PetClinicAppointmentApp.Interfaces;
using PetClinicAppointmentApp.Models;
using PetClinicAppointmentApp.Resources;
using System.Net;

namespace PetClinicAppointmentApp.Controllers
{
    [Route("api/service/schedule")]
    [ApiController]
    public class ServiceScheduleController : ControllerBase
    {
        private readonly IServiceScheduleService serviceScheduleService;
        private readonly IMapper mapper;

        public ServiceScheduleController(IServiceScheduleService serviceScheduleService, IMapper mapper)
        {
            this.serviceScheduleService = serviceScheduleService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("list")]
        public async Task<ActionResult<ResponseResource>> Get([FromQuery] int? service_id, [FromQuery] DateTime? start_schedule)
        {
            var scheduleList = await serviceScheduleService.GetServiceScheduleList(service_id, start_schedule);
            return Ok(new ResponseResource
            {
                Status = new RequestStatus
                {
                    Code = HttpStatusCode.OK,
                    Message = "Berhasil"
                },
                Data = scheduleList
            });
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ResponseResource>> GetDetail(int id)
        {
            var scheduleDetail = serviceScheduleService.GetServiceScheduleDetail(id);
            if(scheduleDetail == null)
            {
                return Ok(new ResponseResource
                {
                    Status = new RequestStatus
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Jadwal layanan tidak ditemukan"
                    }
                });
            }
            return Ok(new ResponseResource
            {
                Status = new RequestStatus
                {
                    Code = HttpStatusCode.OK,
                    Message = "Berhasil"
                },
                Data = scheduleDetail
            });
        }

        [HttpPost]
        public async Task<ActionResult<ResponseResource>> Create(CreateServiceScheduleResource createServiceScheduleResource)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            if(createServiceScheduleResource.Start_schedule >= createServiceScheduleResource.End_schedule)
            {
                return Ok(new ResponseResource
                {
                    Status = new RequestStatus
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Jadwal mulai harus lebih awal dari jadwal berakhir"
                    }
                });
            };
            var schedule = mapper.Map<CreateServiceScheduleResource, ScheduleService>(createServiceScheduleResource);
            var createdSchedule = await serviceScheduleService.CreateServiceSchedule(schedule, createServiceScheduleResource.Repeat_schedule_Week_count);
            return Created("", new ResponseResource
                {
                    Status = new RequestStatus
                    {
                        Code = HttpStatusCode.Created,
                        Message = "Berhasil"
                    },
                    Data = createdSchedule
                }
            );
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ResponseResource>> Update(int id, [FromBody] UpdateServiceScheduleResource updateServiceScheduleResource)
        {
            if (updateServiceScheduleResource.Start_schedule >= updateServiceScheduleResource.End_schedule)
            {
                return Ok(new ResponseResource

                {
                    Status = new RequestStatus
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Jadwal mulai harus lebih awal dari jadwal berakhir"
                    }
                });
            };
            var schedule = mapper.Map<UpdateServiceScheduleResource, ScheduleService>(updateServiceScheduleResource);
            try
            {
                var updatedSchedule = await serviceScheduleService.UpdateServiceSchedule(id, schedule, updateServiceScheduleResource.Quota, updateServiceScheduleResource.Status);
                if (updatedSchedule == null)
                {
                    return Ok(new ResponseResource
                    {
                        Status = new RequestStatus
                        {
                            Code = HttpStatusCode.OK,
                            Message = "Jadwal layanan tidak ditemukan"
                        }
                    });
                }
                return Ok(new ResponseResource
                {
                    Status = new RequestStatus
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Berhasil"
                    },
                    Data = updatedSchedule
                });
            }catch(Exception ex)
            {
                if (ex is InvalidOperationException)
                {
                    return Ok(new ResponseResource
                    {
                        Status = new RequestStatus
                        {
                            Code = HttpStatusCode.OK,
                            Message = "Jadwal Layanan tidak bisa diubah karena sedang terdapat janji temu yang berjalan dengan jadwal layanan ini"
                        }
                    });
                }
                return Ok(new ResponseResource
                {
                    Status = new RequestStatus
                    {
                        Code = HttpStatusCode.OK,
                        Message = ""
                    }
                });
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ResponseResource>> Delete(int id)
        {
            var deletedSchedule = await serviceScheduleService.DeleteServiceSchedule(id);
            if (deletedSchedule == null)
            {
                return Ok(new ResponseResource
                {
                    Status = new RequestStatus
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Jadwal layanan tidak ditemukan"
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
