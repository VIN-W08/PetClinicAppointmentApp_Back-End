using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PetClinicAppointmentApp.Interfaces;
using PetClinicAppointmentApp.Models;
using PetClinicAppointmentApp.Resources;
using System.Net;

namespace PetClinicAppointmentApp.Controllers
{
    [Route("api/service")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        public readonly IServiceService serviceService;
        public readonly IMapper mapper;

        public ServiceController(IServiceService serviceService, IMapper mapper)
        {
            this.serviceService = serviceService;
            this.mapper = mapper;
        }
        
        [HttpPost]
        public async Task<ActionResult<ResponseResource>> Create(CreateServiceResource createServiceResource)
        {
            var service = mapper.Map<CreateServiceResource, Service>(createServiceResource);
            var createdService = await serviceService.CreateService(service);
            var responseResource = new ResponseResource
            {
                Status = new RequestStatus
                {
                    Code = HttpStatusCode.Created,
                    Message = "Berhasil"
                },
                Data = createdService
            };
            return Created("", responseResource);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ResponseResource>> Update(int id, UpdateServiceResource updateServiceResource)
        {
            var service = mapper.Map<UpdateServiceResource, Service>(updateServiceResource);
            try
            {
                var updatedService = await serviceService.UpdateService(id, service, updateServiceResource.Status);
                if (updatedService == null)
                {
                    return Ok(new ResponseResource
                    {
                        Status = new RequestStatus
                        {
                            Code = HttpStatusCode.OK,
                            Message = "Layanan tidak ditemukan"
                        }
                    });
                }
                return Created("", new ResponseResource
                {
                    Status = new RequestStatus
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Berhasil"
                    },
                    Data = updatedService
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
                            Message = "Layanan tidak bisa diubah karena sedang terdapat janji temu yang berjalan dengan layanan ini"
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

        [HttpGet]
        [Route("list")]
        public async Task<ActionResult<ResponseResource>> Get([FromQuery] GetServiceListResource getServiceListResource)
        {
            var serviceList = await serviceService.GetServiceList(getServiceListResource.Clinic_id, getServiceListResource.Name);
           return Ok(new ResponseResource
           {
               Status = new RequestStatus
               {
                   Code = HttpStatusCode.OK,
                   Message = "Berhasil"
               },
               Data = serviceList
           });
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ResponseResource>> GetDetail(int id)
        {
            var selectedService = await serviceService.GetServiceDetail(id);
            if(selectedService == null)
            {
                return Ok(new ResponseResource
                {
                    Status = new RequestStatus
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Layanan tidak ditemukan"
                    }
                });
            }
            var serviceDetailResource = mapper.Map<Service, ServiceDetailResource>(selectedService);
                    
            return Ok(new ResponseResource
            {
                Status = new RequestStatus
                {
                    Code = HttpStatusCode.OK,
                    Message = "Berhasil"
                },
                Data = serviceDetailResource
            });
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ResponseResource>> Delete(int id)
        {
            var deletedService = await serviceService.DeleteService(id);
            if(deletedService == null)
            {
                return Ok(new ResponseResource
                {
                    Status = new RequestStatus
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Layanan tidak ditemukan"
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
                Data = deletedService
            });
        }
    }
}
