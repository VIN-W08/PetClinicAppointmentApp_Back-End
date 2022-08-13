namespace PetClinicAppointmentApp.Resources
{
    public class GetAppointmentListResource
    {
        const int maxPageSize = 20;
        public int? customer_id { get; set; }

        public int? clinic_id { get; set; }

        public byte? status { get; set; }

        public DateTime? start_schedule { get; set; }
        public DateTime? from_schedule { get; set; }

        public bool? finished { get; set; }
        public string? sort_order{ get; set; } = "desc-created_at";
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
