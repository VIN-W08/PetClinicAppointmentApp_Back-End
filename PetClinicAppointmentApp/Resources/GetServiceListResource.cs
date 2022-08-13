using System.ComponentModel.DataAnnotations;

namespace PetClinicAppointmentApp.Resources
{
    public class GetServiceListResource
    {
        public int? Clinic_id { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Name { get; set; } = "";
    }
}
