using PetClinicAppointmentApp.Models;
using System.Net;
using System.Text.Json.Nodes;

namespace PetClinicAppointmentApp.Resources
{
    public class ResponseResource
    {
        public RequestStatus Status { get; set; }
        public object? Data { get; set; }
    }

    public class RequestStatus
    {
        public HttpStatusCode Code { get; set; }
        public string Message { get; set; }
    }
}
