using AutoMapper;
using PetClinicAppointmentApp.Models;
using PetClinicAppointmentApp.Resources;

namespace PetClinicAppointmentApp.Mappers
{
    public class ResourceToModelProfile: Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<CreateAuthResource, Customer>();
            CreateMap<UpdateCustomerResource, Customer>();
            CreateMap<RegisterPetClinicResource, PetClinic>();
            CreateMap<UpdatePetClinicResource, PetClinic>();
            CreateMap<CreateServiceResource, Service>();
            CreateMap<UpdateServiceResource, Service>();
            CreateMap<CreateServiceScheduleResource, ScheduleService>();
            CreateMap<UpdateServiceScheduleResource, ScheduleService>();
            CreateMap<CreateClinicScheduleResource, SchedulePetClinic>();
            CreateMap<UpdateClinicScheduleResource, SchedulePetClinic>();
            CreateMap<CreateAppointmentResource, Appointment>();
        }
    }
}
