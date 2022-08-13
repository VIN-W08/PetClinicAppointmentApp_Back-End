using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PetClinicAppointmentApp.Interfaces;
using PetClinicAppointmentApp.Models;
using PetClinicAppointmentApp.Resources;
using System.Net;

namespace PetClinicAppointmentApp.Controllers
{
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService customerService;
        private readonly IMapper mapper;
        public CustomerController(ICustomerService customerService, IMapper mapper)
        {
            this.customerService = customerService;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseResource>> Register([FromBody] RegisterCustomerResource registerCustomerResource)
        {
            try
            {
                var customer = await customerService.CreateCustomer(
                        registerCustomerResource.Name,
                        registerCustomerResource.Email,
                        registerCustomerResource.Password
                    );

                var authCustomerResource = mapper.Map<Customer, AuthCustomerResource>(customer);
                var customerAuthResource = new CustomerAuthResource
                {
                    Status = true,
                    Customer = authCustomerResource,
                    Role = "customer",
                    Created_at = DateTime.Now
                };
                return Created("", new ResponseResource
                {
                    Status = new RequestStatus
                    {
                        Code = HttpStatusCode.Created,
                        Message = "Berhasil"
                    },
                    Data = customerAuthResource
                });
            } catch(Exception ex)
            {
                if (ex is ArgumentException)
                {
                    return Ok(new ResponseResource
                    {
                        Status = new RequestStatus
                        {
                            Code = HttpStatusCode.OK,
                            Message = "Email telah digunakan"
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
        [Route("{id}")]
        public async Task<ActionResult<ResponseResource>> GetDetail(int id)
        {
            var customer = await customerService.GetCustomerById(id);
            if(customer == null)
            {
                return Ok(new ResponseResource
                {
                    Status = new RequestStatus
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Pengguna tidak ditemukan"
                    }

                });
            }
            var customerResource = mapper.Map<Customer, CustomerResource>(customer);
            return Ok(new ResponseResource
            {
                Status = new RequestStatus
                {
                    Code = HttpStatusCode.OK,
                    Message = "Berhasil"
                },
                Data = customerResource
            });
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ResponseResource>> Update(
            int id,
            [FromBody] UpdateCustomerResource updateCustomerResource
        )
        {
            try
            {
                var customer = mapper.Map<UpdateCustomerResource, Customer>(updateCustomerResource);
                var updatedCustomer = await customerService.UpdateCustomer(
                    id,
                    customer
                );
                if (updatedCustomer == null)
                {
                    return Ok(new ResponseResource
                    {
                        Status = new RequestStatus
                        {
                            Code = HttpStatusCode.OK,
                            Message = "Pengguna tidak ditemukan"
                        }
                    });
                }
                var customerResource = mapper.Map<Customer, CustomerResource>(updatedCustomer);
                return Ok(new ResponseResource
                {
                    Status = new RequestStatus
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Berhasil"
                    },
                    Data = customerResource
                });
            }catch(Exception ex)
            {
                if (ex is ArgumentException)
                {
                    return Ok(new ResponseResource
                    {
                        Status = new RequestStatus
                        {
                            Code = HttpStatusCode.OK,
                            Message = "Email telah digunakan"
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

        [HttpPatch]
        [Route("password")]
        public async Task<ActionResult<ResponseResource>> UpdatePassword([FromBody] UpdatePasswordResource updatePasswordResource)
        {
            var updatedCustomer = await customerService.ChangePassword(updatePasswordResource.Email, updatePasswordResource.Password);
            if(updatedCustomer == null)
            {
                return Ok(new ResponseResource
                {
                    Status = new RequestStatus
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Pengguna tidak ditemukan"
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
                Data = new {}
            });
        }
    }
}
