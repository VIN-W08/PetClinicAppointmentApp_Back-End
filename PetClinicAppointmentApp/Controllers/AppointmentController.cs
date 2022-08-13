using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PetClinicAppointmentApp.Interfaces;
using PetClinicAppointmentApp.Models;
using PetClinicAppointmentApp.Resources;
using System.Net;

namespace PetClinicAppointmentApp.Controllers
{
    [Route("api/appointment")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService appointmentService;
        private readonly IServiceScheduleService serviceScheduleService;
        private readonly IMapper mapper;

        public AppointmentController(IAppointmentService appointmentService, IServiceScheduleService serviceScheduleService, IMapper mapper)
        {
            this.appointmentService = appointmentService;
            this.serviceScheduleService = serviceScheduleService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("list")] 
        public async Task<ActionResult<ResponseResource>> Get(
            [FromQuery] GetAppointmentListResource getAppointmentListResource
        )
        {   
            var appointmentList = await appointmentService.GetAppointmentList(
                getAppointmentListResource.clinic_id,
                getAppointmentListResource.customer_id,
                getAppointmentListResource.status,
                getAppointmentListResource.start_schedule,
                getAppointmentListResource.from_schedule,
                getAppointmentListResource.finished,
                getAppointmentListResource.sort_order,
                getAppointmentListResource.Page_number,
                getAppointmentListResource.Page_size
            );

            var appointmentListResource = mapper.Map<List<Appointment>, List<AppointmentResource>>(appointmentList);
            appointmentListResource = appointmentListResource.Select((r, idx) =>
            {
                if (appointmentList[idx].Pet_clinic.Image_name != null)
                {
                    r.Pet_clinic.Image = GetImageFromFileSystemByPath("\\Images\\Clinic\\", appointmentList[idx].Pet_clinic.Image_name);
                }
                return r;
            }).ToList();
            return Ok(new ResponseResource
            {
                Status = new RequestStatus
                {
                    Code = HttpStatusCode.OK,
                    Message = "Berhasil"
                },
                Data = appointmentListResource
            });
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ResponseResource>> GetDetail(int id)
        {
            var appointment = await appointmentService.GetAppointmentDetail(id);
            if(appointment == null)
            {
                return Ok(new ResponseResource
                {
                    Status = new RequestStatus
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Janji temu tidak ditemukan"
                    }
                });
            }
            var appointmentResource = mapper.Map<Appointment, AppointmentResource>(appointment);
            if (appointment.Pet_clinic.Image_name != null) {
                appointmentResource.Pet_clinic.Image = GetImageFromFileSystemByPath("\\Images\\Clinic\\", appointment.Pet_clinic.Image_name);
            }
            return Ok(new ResponseResource
            {
                Status = new RequestStatus
                {
                    Code = HttpStatusCode.OK,
                    Message = "Berhasil"
                },
                Data = appointmentResource
            });

        }

        [HttpPost]
        public async Task<ActionResult<ResponseResource>> Create(CreateAppointmentResource createAppointmentResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseResource
                {
                    Status = new RequestStatus
                    {
                        Code = HttpStatusCode.BadRequest,
                        Message = ""
                    }
                });
            }
            var appointment = mapper.Map<CreateAppointmentResource, Appointment>(createAppointmentResource);
            if(appointment.Schedule_service_id == null)
            {
                BadRequest(new ResponseResource     
                {
                    Status = new RequestStatus
                    {
                        Code = HttpStatusCode.BadRequest,
                        Message = "Jadwal layanan wajib diinput"
                    }
                });
            }
            var selectedServiceSchedule = serviceScheduleService.GetServiceScheduleDetail((int) appointment.Schedule_service_id);
            if(selectedServiceSchedule == null)
            {
                return Ok(new ResponseResource
                {
                    Status = new RequestStatus
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Jadwal layanan tidak tersedia"
                    }
                });
            }
            if (selectedServiceSchedule.Quota.Equals(0))
            {
                return Ok(new ResponseResource
                {
                    Status = new RequestStatus
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Jadwal Layanan tidak memiliki kuota yang tersedia"
                    }
                });
            }
            var createdAppointment = await appointmentService.CreateAppointment(appointment);
            var createdAppointmentDetailResource = mapper.Map<Appointment, AppointmentResource>(createdAppointment);
            return Ok(new ResponseResource
            {
                Status = new RequestStatus
                {
                    Code = HttpStatusCode.Created,
                    Message = "Berhasil"
                },
                Data = createdAppointmentDetailResource
            });
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<ActionResult<ResponseResource>> UpdateAppointmentStatus(int id, [FromBody] UpdateAppointmentStatusResource updateAppointmentStatusResource)
        {
            if(updateAppointmentStatusResource.status.Equals(3))
            {
                var selectedAppointment = await appointmentService.GetAppointmentDetail(id);
                if(selectedAppointment == null)
                {
                    return new ResponseResource
                    {
                        Status = new RequestStatus
                        {
                            Code = HttpStatusCode.OK,
                            Message = "Janji temu tidak ditemukan"
                        }
                    };
                }
                if (DateTime.Now >= selectedAppointment.Schedule_service.Start_schedule.Subtract(new TimeSpan(0, 0, 30, 0)))
                {
                    return new ResponseResource
                    {
                        Status = new RequestStatus
                        {
                            Code = HttpStatusCode.OK,
                            Message = "Janji temu tidak bisa dibatalkan"
                        }
                    };
                }
            }
            var updatedAppointment = await appointmentService.UpdateAppointmentStatus(id, updateAppointmentStatusResource.status);
            if(updatedAppointment == null)
            {
                return Ok(new ResponseResource
                {
                    Status = new RequestStatus
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Janji temu tidak ditemukan"
                    }
                });
            }
            switch (updateAppointmentStatusResource.status)
            {
                case 1:
                    await serviceScheduleService.AddSubstractServiceScheduleQuota((int)updatedAppointment.Schedule_service_id, -1);
                    break;
                case 3:
                    if (updateAppointmentStatusResource.status.Equals(1))
                    {
                        await serviceScheduleService.AddSubstractServiceScheduleQuota((int)updatedAppointment.Schedule_service_id, 1);
                    }
                    break;
                default: break;
            }
            var appointmentResource = mapper.Map<Appointment, AppointmentResource>(updatedAppointment);
            if (updatedAppointment.Pet_clinic.Image_name != null)
            {
                appointmentResource.Pet_clinic.Image = GetImageFromFileSystemByPath("\\Images\\Clinic\\", updatedAppointment.Pet_clinic.Image_name);
            }
            return new ResponseResource
            {
                Status = new RequestStatus
                {
                    Code = HttpStatusCode.OK,
                    Message = "Berhasil"
                },
                Data = appointmentResource
            };
        }
        private byte[] GetImageFromFileSystemByPath(string dir, string imageName)
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory() + dir);
            var imagePath = Path.Combine(basePath, imageName);
            var imageBytes = System.IO.File.ReadAllBytes(imagePath);

            return imageBytes;
        }
    }
}
