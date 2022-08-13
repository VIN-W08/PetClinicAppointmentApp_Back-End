namespace PetClinicAppointmentApp.Resources
{
    public class UpdateServiceResource
    {
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public bool? Status { get; set; } = null;
    }
}
