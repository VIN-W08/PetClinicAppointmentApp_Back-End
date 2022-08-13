using Microsoft.EntityFrameworkCore;
using PetClinicAppointmentApp.Interfaces;
using PetClinicAppointmentApp.Models;
using System.Device.Location;

namespace PetClinicAppointmentApp.Services
{
    public class PetClinicService: IPetClinicService
    {
        private readonly PetClinicAppointmentDbContext petClinicAppointmentDbContext;
        public PetClinicService(PetClinicAppointmentDbContext petClinicAppointmentDbContext)
        {
            this.petClinicAppointmentDbContext = petClinicAppointmentDbContext;
        }
        public async Task<bool> IsEmailExist(int id, string email)
        {
            return (
                await petClinicAppointmentDbContext.PetClinics.AnyAsync(petClinic => petClinic.Pet_clinic_id != id ? petClinic.Email == email : false)
            );
        }

        public async Task<PetClinic> CreatePetClinic(PetClinic petClinic, IFormFile? image)
        {
            if(await IsEmailExist(petClinic.Pet_clinic_id, petClinic.Email))
            {
                throw new ArgumentException();
            }
            petClinic.Password = BCrypt.Net.BCrypt.HashPassword(petClinic.Password);
            if (image != null)
            {
                var baseImagePath = Path.Combine(Directory.GetCurrentDirectory() + "\\Images\\Clinic\\");
                bool baseImagePathExist = Directory.Exists(baseImagePath);
                if (!baseImagePathExist) Directory.CreateDirectory(baseImagePath);
                var currentDateTime = DateTime.Now;
                var imageName =
                    currentDateTime.Hour.ToString() +
                    currentDateTime.Minute.ToString() +
                    currentDateTime.Second.ToString() +
                    currentDateTime.Millisecond.ToString() +
                    currentDateTime.Date.Day.ToString() +
                    currentDateTime.Month.ToString() +
                    currentDateTime.Year.ToString() +
                    "_" + Path.GetFileNameWithoutExtension(image.FileName).ToLower().Replace(" ", "_");
                var imageExtension = Path.GetExtension(image.FileName);
                var imagePath = Path.Combine(baseImagePath, imageName + imageExtension);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
                petClinic.Image_name = $"{imageName}{imageExtension}";
            }
            petClinicAppointmentDbContext.PetClinics.Add(petClinic);
            await petClinicAppointmentDbContext.SaveChangesAsync();
            return petClinic;
        }

        public async Task<PetClinic?> UpdatePetClinic(int petClinicId, PetClinic petClinic, IFormFile? image)
        {
            var selectedPetClinic = await petClinicAppointmentDbContext.PetClinics.FindAsync(petClinicId);
            if(selectedPetClinic == null)
            {
                return null;
            }
            if (await IsEmailExist(petClinicId, petClinic.Email))
            {
                throw new ArgumentException();
            }
            if (!string.IsNullOrEmpty(petClinic.Name))
            {
                selectedPetClinic.Name = petClinic.Name;
            }
            if (!string.IsNullOrEmpty(petClinic.Email))
            {
                selectedPetClinic.Email = petClinic.Email;
            }
            if (!string.IsNullOrEmpty(petClinic.Phone_number))
            {
                selectedPetClinic.Phone_number = petClinic.Phone_number;
            }
            if (!string.IsNullOrEmpty(petClinic.Address))
            {
                selectedPetClinic.Address = petClinic.Address;
            }
            if (petClinic.Village_id != null)
            {
               selectedPetClinic.Village_id = petClinic.Village_id;
            }
            if (petClinic.Latitude != null)
            {
                selectedPetClinic.Latitude = petClinic.Latitude;
            }
            if (petClinic.Longitude != null)
            {
                selectedPetClinic.Longitude = petClinic.Longitude;
            }
            if(image != null)
            {
                var baseImagePath = Path.Combine(Directory.GetCurrentDirectory() + "\\Images\\Clinic\\");
                bool baseImagePathExist = Directory.Exists(baseImagePath);
                if (!baseImagePathExist) Directory.CreateDirectory(baseImagePath);
                var currentDateTime = DateTime.Now;
                var imageName =
                    currentDateTime.Hour.ToString() +
                    currentDateTime.Minute.ToString() +
                    currentDateTime.Second.ToString() +
                    currentDateTime.Millisecond.ToString() +
                    currentDateTime.Date.Day.ToString() +
                    currentDateTime.Month.ToString() +
                    currentDateTime.Year.ToString() +
                    "_" + Path.GetFileNameWithoutExtension(image.FileName).ToLower().Replace(" ", "_");
                var imageExtension = Path.GetExtension(image.FileName);
                var imagePath = Path.Combine(baseImagePath, imageName + imageExtension);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
                
                var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\Images\\Clinic\\");
                if (File.Exists(basePath + selectedPetClinic.Image_name))
                {
                    File.Delete(basePath + selectedPetClinic.Image_name);
                }
                selectedPetClinic.Image_name = $"{imageName}{imageExtension}";
            }
            await petClinicAppointmentDbContext.SaveChangesAsync();
            return selectedPetClinic;
        }

        public async Task<PetClinic?> DeletePetClinicImage(int petClinicId)
        {
            var selectedPetClinic = await petClinicAppointmentDbContext.PetClinics.FindAsync(petClinicId);
            if (selectedPetClinic == null)
            {
                return null;
            }
            var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\Images\\Clinic\\");
            var imagePath = basePath + selectedPetClinic.Image_name;
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
            selectedPetClinic.Image_name = null;
          
            await petClinicAppointmentDbContext.SaveChangesAsync();
            return selectedPetClinic;
        }

        public async Task<List<PetClinic>> GetPetClinicList(
            int pageNumber, 
            int pageSize, 
            double? latitude, 
            double? longitude,
            string name
        )
        {
           var clinicListAsc = new List<PetClinic>();
           clinicListAsc = petClinicAppointmentDbContext.PetClinics
                      .Where(clinic => clinic.Name.Trim().ToLower().Contains(name.Trim().ToLower()))
                      .AsEnumerable()
                      .Select(clinic =>
                {
                    clinic.Distance = (new GeoCoordinate(clinic.Latitude, clinic.Longitude)
                           .GetDistanceTo(new GeoCoordinate(
                               (double)latitude,
                               (double)longitude
                               )
                           ))/1000;
                          return clinic;
                      })
                      .Where(clinic => clinic.Distance <= 15)
                      .OrderBy(clinic => clinic.Distance)
                      .AsQueryable()
                      .Skip((pageNumber - 1) * pageSize)
                      .Take(pageSize)
                      .ToList();
            return clinicListAsc;
        }

        public async Task<PetClinic> GetPetClinicDetail(int petClinicId)
        {
            var petClinic = await petClinicAppointmentDbContext.PetClinics
                .Where(pc => pc.Pet_clinic_id == petClinicId)
                .Include(pc => pc.Services)
                .Include(pc => pc.Schedules_pet_clinic)
                .FirstOrDefaultAsync();
            return petClinic;
        }

        public async Task<PetClinic?> ChangePassword(string email, string newPassword)
        {
            var selectedClinic = await petClinicAppointmentDbContext.PetClinics.SingleOrDefaultAsync(clinic => clinic.Email.Equals(email));
            if (selectedClinic == null)
            {
                return null;
            }
            selectedClinic.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await petClinicAppointmentDbContext.SaveChangesAsync();
            return selectedClinic;
        }

        public async Task<PetClinic?> UpdateClinicStatus(int petClinicId, bool status)
        {
            var selectedClinic = await petClinicAppointmentDbContext.PetClinics
                .SingleOrDefaultAsync(clinic => clinic.Pet_clinic_id == petClinicId);
            if(selectedClinic == null)
            {
                return null;
            }
            selectedClinic.Status = status;
            await petClinicAppointmentDbContext.SaveChangesAsync();
            return selectedClinic;
        }
    }
}
