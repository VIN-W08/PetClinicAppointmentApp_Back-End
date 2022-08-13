using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PetClinicAppointmentApp.Interfaces;
using PetClinicAppointmentApp.Models;
using PetClinicAppointmentApp.Resources;
using System.Net;

namespace PetClinicAppointmentApp.Controllers
{
    [Route("api/customer/auth")]
    [ApiController]
    public class AuthCustomerController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly IMapper mapper;
        public AuthCustomerController(IAuthService authService, IMapper mapper)
        {
            this.authService = authService;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseResource>> Auth([FromBody] CreateAuthResource createAuthResource)
        {
            var customer = await authService.AuthCustomer(createAuthResource.Email, createAuthResource.Password);
            if (customer == null)
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
            var authCustomerResource = mapper.Map<Customer, AuthCustomerResource>(customer);
            var customerAuthResource = new CustomerAuthResource
            {
                Status = true,
                Customer = authCustomerResource,
                Role = "customer",
                Created_at = DateTime.Now
            };
            var responseResource = new ResponseResource
            {
                Status = new RequestStatus
                {
                    Code = HttpStatusCode.OK,
                    Message = "Successful"
                },
                Data = customerAuthResource
            };
            return Ok(responseResource);
        }
    }
}
