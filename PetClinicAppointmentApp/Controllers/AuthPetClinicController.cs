using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PetClinicAppointmentApp.Interfaces;
using PetClinicAppointmentApp.Models;
using PetClinicAppointmentApp.Resources;
using System.Net;

namespace PetClinicAppointmentApp.Controllers
{
    [Route("api/petclinic/auth")]
    [ApiController]
    public class AuthPetClinicController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly IMapper mapper;
        public AuthPetClinicController(IAuthService authService, IMapper mapper)
        {
            this.authService = authService;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseResource>> Auth([FromBody] CreateAuthResource createAuthResource)
        {
            var clinic = await authService.AuthPetClinic(createAuthResource.Email, createAuthResource.Password);
            if (clinic == null)
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
            var authPetClinicResource = mapper.Map<PetClinic, AuthPetClinicResource>(clinic);
            if (clinic.Image_name != null)
            {
                authPetClinicResource.Image = GetImageFromFileSystemByPath("\\Images\\Clinic\\", clinic.Image_name);
            }
            var clinicAuthResource = new PetClinicAuthResource
            {
                Status = true,
                Pet_clinic = authPetClinicResource,
                Role = "clinic",
                Created_at = DateTime.Now
            };
            var responseResource = new ResponseResource
            {
                Status = new RequestStatus
                {
                    Code = HttpStatusCode.OK,
                    Message = "Successful"
                },
                Data = clinicAuthResource
            };
            return Ok(responseResource);
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
