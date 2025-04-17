using System;
namespace casus_ouderenzorg.Models
{
    public class TransportView
    {
        public int TransportID { get; set; }
        public DateTime TransportDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan ReturnTime { get; set; }
        public string Departure { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public string? Reason { get; set; }
        public string? PatientName { get; set; }
        public string? DriverName { get; set; }
        public string? VehicleName { get; set; }
    }
}
