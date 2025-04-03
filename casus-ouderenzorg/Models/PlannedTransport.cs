namespace casus_ouderenzorg.Models
{
    public class PlannedTransport
    {
        public PlannedTransport()
        {
            Passengers = new List<Passenger>();
        }

        public int TransportId { get; set; }
        public string RitNumber { get; set; }
        public DateTime DepartureTime { get; set; }
        public string DepartureAddress { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string ArrivalAddress { get; set; }
        public string Vehicle { get; set; }
        public string Driver { get; set; }
        public List<Passenger> Passengers { get; set; }
    }
}
