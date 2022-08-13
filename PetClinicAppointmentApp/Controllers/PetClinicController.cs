using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PetClinicAppointmentApp.Interfaces;
using PetClinicAppointmentApp.Models;
using PetClinicAppointmentApp.Resources;
using System.Net;

namespace PetClinicAppointmentApp.Controllers
{
    [Route("api/petclinic")]
    [ApiController]
    public class PetClinicController : ControllerBase
    {
        private readonly IPetClinicService petClinicService;
        private readonly IMapper mapper;
        public PetClinicController(IPetClinicService petClinicService, IMapper mapper)
        {
            this.petClinicService = petClinicService;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseResource>> Register([FromForm] RegisterPetClinicResource form)
        {
            try
            {
                var petClinic = mapper.Map<RegisterPetClinicResource, PetClinic>(form);
                var createdPetClinic = await petClinicService.CreatePetClinic(petClinic, form.Image);
                var authPetClinicResource = mapper.Map<PetClinic, AuthPetClinicResource>(createdPetClinic);
                if (createdPetClinic.Image_name != null)
                {
                    authPetClinicResource.Image = GetImageFromFileSystemByPath("\\Images\\Clinic\\", createdPetClinic.Image_name);
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
                        Code = HttpStatusCode.Created,
                        Message = "Berhasil"
                    },
                    Data = clinicAuthResource
                };
                return Created("", responseResource);
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

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ResponseResource>> Update(int id, [FromForm] UpdatePetClinicResource updatePetClinicResource)
        {
            try
            {
                var petClinic = mapper.Map<UpdatePetClinicResource, PetClinic>(updatePetClinicResource);
                var updatedPetClinic = await petClinicService.UpdatePetClinic(id, petClinic, updatePetClinicResource.Image);
                if (updatedPetClinic == null)
                {
                    return new ResponseResource
                    {
                        Status = new RequestStatus
                        {
                            Code = HttpStatusCode.OK,
                            Message = "Pengguna tidak ditemukan"
                        }
                    };
                }
                var petClinicResource = mapper.Map<PetClinic, PetClinicResource>(updatedPetClinic);
                if (updatedPetClinic.Image_name != null)
                {
                    petClinicResource.Image = GetImageFromFileSystemByPath("\\Images\\Clinic\\", updatedPetClinic.Image_name);
                }
                var responseResource = new ResponseResource
                {
                    Status = new RequestStatus
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Berhasil"
                    },
                    Data = petClinicResource
                };
                return Ok(responseResource);
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
        [Route("{id}")]
        public async Task<ActionResult<ResponseResource>> DeletePetClinicImage(int id)
        {
            var updatedPetClinic = await petClinicService.DeletePetClinicImage(id);
            if (updatedPetClinic == null)
            {
                return new ResponseResource
                {
                    Status = new RequestStatus
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Pengguna tidak ditemukan"
                    }
                };
            }
            var petClinicResource = mapper.Map<PetClinic, PetClinicResource>(updatedPetClinic);
            var responseResource = new ResponseResource
            {
                Status = new RequestStatus
                {
                    Code = HttpStatusCode.OK,
                    Message = "Berhasil"
                },
                Data = petClinicResource
            };
            return Ok(responseResource);
        }

        [HttpGet]
        [Route("list")]
        public async Task<ActionResult<ResponseResource>> Get([FromQuery] GetPetClinicListResource getPetClinicListResource)
        {
            var petClinicList = await petClinicService.GetPetClinicList(
                getPetClinicListResource.Page_number,
                getPetClinicListResource.Page_size,
                getPetClinicListResource.Latitude,
                getPetClinicListResource.Longitude,
                getPetClinicListResource.Name
            );

            var petClinicListResource = mapper.Map<List<PetClinic>, List<PetClinicResource>>(petClinicList);
            petClinicListResource = petClinicListResource.Select((r, idx) =>
            {
                if (petClinicList[idx].Image_name != null)
                {
                    r.Image = GetImageFromFileSystemByPath("\\Images\\Clinic\\", petClinicList[idx].Image_name);
                }
                return r;
            }).ToList();

            var responseResource = new ResponseResource
            {
                Status = new RequestStatus
                {
                    Code = HttpStatusCode.OK,
                    Message = "Berhasil"
                },
                Data = petClinicListResource
            };
            return Ok(responseResource);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ResponseResource>> GetDetail(int id)
        {
            var petClinic = await petClinicService.GetPetClinicDetail(id);
            var petClinicDetailResource = mapper.Map<PetClinic, PetClinicDetailResource>(petClinic);
            if (petClinic.Image_name != null)
            {
                petClinicDetailResource.Image = GetImageFromFileSystemByPath("\\Images\\Clinic\\", petClinic.Image_name);
            }
            var responseResource = new ResponseResource
            {
                Status = new RequestStatus
                {
                    Code = HttpStatusCode.OK,
                    Message = "Berhasil"
                },
                Data = petClinicDetailResource
            };
            return Ok(responseResource);
        }

        [HttpPatch]
        [Route("password")]
        public async Task<ActionResult<ResponseResource>> UpdatePassword([FromBody] UpdatePasswordResource updatePasswordResource)
        {
            var updatedClinic = await petClinicService.ChangePassword(updatePasswordResource.Email, updatePasswordResource.Password);
            if (updatedClinic == null)
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

        [HttpPatch]
        [Route("{id}/status")]
        public async Task<ActionResult<ResponseResource>> UpdateStatus(int id, [FromBody] UpdatePetClinicStatusResource updatePetClinicStatusResource)
        {
            var updatedClinic = await petClinicService.UpdateClinicStatus(id, updatePetClinicStatusResource.Status);
            if (updatedClinic == null)
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
            var petClinicResource= mapper.Map<PetClinic, PetClinicResource>(updatedClinic);
            return Ok(new ResponseResource
            {
                Status = new RequestStatus
                {
                    Code = HttpStatusCode.OK,
                    Message = "Berhasil"
                },
                Data = petClinicResource
            });
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
