using System.ComponentModel.DataAnnotations;

namespace PetClinicAppointmentApp.Resources
{
    public class GetPetClinicListResource
    {
        const int maxPageSize = 20; 

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Name { get; set; } = "";
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int _Page_size { get; set; } = 10;
        public int Page_size
        {
            get
            {
                return _Page_size;
            }

            set
            {
               _Page_size = (value > maxPageSize) ? maxPageSize : value;
            }
        }
        public int Page_number { get; set; } = 1;
    }
}
