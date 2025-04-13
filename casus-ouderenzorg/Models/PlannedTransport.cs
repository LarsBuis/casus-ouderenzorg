using casus_ouderenzorg.Models;
using System;
using System.Collections.Generic;

namespace casus_ouderenzorg.Models
{
    public class PlannedTransport
    {
        public int Id { get; set; }
        public string RitNumber { get; set; }
        public DateTime DepartureTime { get; set; }
        public string DepartureAddress { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string ArrivalAddress { get; set; }

        // Selected entities – these will be set from the lists read from the DB.
        public Driver SelectedDriver { get; set; }
        public Vehicle SelectedVehicle { get; set; }
        public Caregiver SelectedCaregiver { get; set; }

        // The list of passengers (each wraps a Patient)
        public List<Passenger> Passengers { get; set; } = new List<Passenger>();

        // Maximum passengers allowed is the vehicle capacity minus one (reserved for the caregiver)
        public int MaxPassengers => SelectedVehicle != null ? SelectedVehicle.Capacity - 1 : 0;

        // Returns how many more passengers can be added.
        public int RemainingPassengerSpots()
        {
            return MaxPassengers - Passengers.Count;
        }

        // Adds a patient as a passenger provided there is enough capacity.
        public bool AddPassenger(Patient patient)
        {
            if (RemainingPassengerSpots() > 0)
            {
                var passenger = new Passenger
                {
                    Id = Passengers.Count + 1, // simple incremental ID; adjust as needed
                    Patient = patient
                };
                Passengers.Add(passenger);
                return true;
            }
            return false;
        }
    }
}
