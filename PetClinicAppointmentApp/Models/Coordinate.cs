namespace PetClinicAppointmentApp.Models
{
    public class Coordinate
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Coordinate(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }
    }
}
