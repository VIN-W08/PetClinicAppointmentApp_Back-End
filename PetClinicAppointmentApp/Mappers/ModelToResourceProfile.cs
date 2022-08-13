using AutoMapper;
using PetClinicAppointmentApp.Models;
using PetClinicAppointmentApp.Resources;

namespace PetClinicAppointmentApp.Mappers
{
    public class ModelToResourceProfile: Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Customer, CustomerResource>();
            CreateMap<Customer, AuthCustomerResource>();
            CreateMap<PetClinic, PetClinicResource>();
            CreateMap<PetClinic, PetClinicDetailResource>();
            CreateMap<PetClinic, AuthPetClinicResource>();
            CreateMap<Service, ServiceDetailResource>();
            CreateMap<Appointment, ListAppointmentResource>();
            CreateMap<Appointment, AppointmentResource>();
        }
    }
}
