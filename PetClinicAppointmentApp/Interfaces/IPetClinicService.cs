using PetClinicAppointmentApp.Models;

namespace PetClinicAppointmentApp.Interfaces
{
    public interface IPetClinicService
    {
        Task<PetClinic> CreatePetClinic(PetClinic petClinic, IFormFile? image);
        Task<PetClinic?> UpdatePetClinic(int petClinicId, PetClinic petClinic, IFormFile? image);
        Task<PetClinic?> DeletePetClinicImage(int petClinicId);
        Task<List<PetClinic>> GetPetClinicList(int pageNumber, int pageSize, double? latitude, double? longitude, string name = "");
        Task<PetClinic> GetPetClinicDetail(int petClinicId);
        Task<PetClinic?> ChangePassword(string email, string newPassword);
        Task<PetClinic?> UpdateClinicStatus(int petClinicId, bool status);
    }
}
