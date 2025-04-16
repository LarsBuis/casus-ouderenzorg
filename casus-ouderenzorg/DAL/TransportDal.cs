using casus_ouderenzorg.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace casus_ouderenzorg.DAL
{
    public class TransportRepository
    {
        private readonly string _connectionString;
        public TransportRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Driver> GetDrivers()
        {
            var drivers = new List<Driver>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT Id, Name FROM Drivers";
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        drivers.Add(new Driver
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }
            }
            return drivers;
        }

        public List<Vehicle> GetVehicles()
        {
            var vehicles = new List<Vehicle>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT Id, Name, Capacity FROM Vehicles";
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        vehicles.Add(new Vehicle
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Capacity = reader.GetInt32(2)
                        });
                    }
                }
            }
            return vehicles;
        }

        public List<Caregiver> GetCaregivers()
        {
            var caregivers = new List<Caregiver>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT Id, Name FROM Caregivers";
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        caregivers.Add(new Caregiver
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }
            }
            return caregivers;
        }

        public List<Patient> GetPatients()
        {
            var patients = new List<Patient>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT Id, Name FROM Patients";
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        patients.Add(new Patient
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }
            }
            return patients;
        }

        // Inserts a PlannedTransport and its associated passengers in a transaction.
        // Assumes PlannedTransports has columns for transport details and foreign keys for SelectedDriver, SelectedVehicle, and SelectedCaregiver.
        // The Passengers table should have PlannedTransportId and PatientId.
        public int AddPlannedTransport(PlannedTransport transport)
        {
            int newId = 0;
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string insertTransport = @"
INSERT INTO PlannedTransports 
    (RitNumber, DepartureTime, DepartureAddress, ArrivalTime, ArrivalAddress, SelectedDriverId, SelectedVehicleId, SelectedCaregiverId)
VALUES 
    (@RitNumber, @DepartureTime, @DepartureAddress, @ArrivalTime, @ArrivalAddress, @DriverId, @VehicleId, @CaregiverId);
SELECT CAST(SCOPE_IDENTITY() AS int);";

                        using (var cmd = new SqlCommand(insertTransport, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@RitNumber", transport.RitNumber);
                            cmd.Parameters.AddWithValue("@DepartureTime", transport.DepartureTime);
                            cmd.Parameters.AddWithValue("@DepartureAddress", transport.DepartureAddress);
                            cmd.Parameters.AddWithValue("@ArrivalTime", transport.ArrivalTime);
                            cmd.Parameters.AddWithValue("@ArrivalAddress", transport.ArrivalAddress);
                            cmd.Parameters.AddWithValue("@DriverId", transport.SelectedDriver?.Id ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@VehicleId", transport.SelectedVehicle?.Id ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@CaregiverId", transport.SelectedCaregiver?.Id ?? (object)DBNull.Value);
                            newId = (int)cmd.ExecuteScalar();
                        }

                        // Insert each passenger for the planned transport.
                        if (transport.Passengers != null && transport.Passengers.Count > 0)
                        {
                            foreach (var passenger in transport.Passengers)
                            {
                                string insertPassenger = @"
INSERT INTO Passengers (PlannedTransportId, PatientId)
VALUES (@PlannedTransportId, @PatientId)";
                                using (var cmdPassenger = new SqlCommand(insertPassenger, conn, transaction))
                                {
                                    cmdPassenger.Parameters.AddWithValue("@PlannedTransportId", newId);
                                    cmdPassenger.Parameters.AddWithValue("@PatientId", passenger.Patient.Id);
                                    cmdPassenger.ExecuteNonQuery();
                                }
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            return newId;
        }

        // NEW: Retrieve all planned transports along with related Driver, Vehicle, and Caregiver data.
        // Then load associated passengers for each transport.
        public List<PlannedTransport> GetPlannedTransports()
        {
            var transports = new List<PlannedTransport>();

            // Use a join query to retrieve PlannedTransports along with Driver, Vehicle and Caregiver
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
SELECT 
    pt.Id,
    pt.RitNumber,
    pt.DepartureTime,
    pt.DepartureAddress,
    pt.ArrivalTime,
    pt.ArrivalAddress,
    d.Id AS DriverId,
    d.Name AS DriverName,
    v.Id AS VehicleId,
    v.Name AS VehicleName,
    v.Capacity,
    c.Id AS CaregiverId,
    c.Name AS CaregiverName
FROM PlannedTransports pt
LEFT JOIN Drivers d ON pt.SelectedDriverId = d.Id
LEFT JOIN Vehicles v ON pt.SelectedVehicleId = v.Id
LEFT JOIN Caregivers c ON pt.SelectedCaregiverId = c.Id";

                using (var cmd = new SqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var transport = new PlannedTransport
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                RitNumber = reader.GetString(reader.GetOrdinal("RitNumber")),
                                DepartureTime = reader.GetDateTime(reader.GetOrdinal("DepartureTime")),
                                DepartureAddress = reader.GetString(reader.GetOrdinal("DepartureAddress")),
                                ArrivalTime = reader.GetDateTime(reader.GetOrdinal("ArrivalTime")),
                                ArrivalAddress = reader.GetString(reader.GetOrdinal("ArrivalAddress"))
                            };

                            if (!reader.IsDBNull(reader.GetOrdinal("DriverId")))
                            {
                                transport.SelectedDriver = new Driver
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("DriverId")),
                                    Name = reader.GetString(reader.GetOrdinal("DriverName"))
                                };
                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("VehicleId")))
                            {
                                transport.SelectedVehicle = new Vehicle
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("VehicleId")),
                                    Name = reader.GetString(reader.GetOrdinal("VehicleName")),
                                    Capacity = reader.GetInt32(reader.GetOrdinal("Capacity"))
                                };
                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("CaregiverId")))
                            {
                                transport.SelectedCaregiver = new Caregiver
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("CaregiverId")),
                                    Name = reader.GetString(reader.GetOrdinal("CaregiverName"))
                                };
                            }

                            // Add the transport to the list; we load its passengers later.
                            transports.Add(transport);
                        }
                    }
                }
            }

            // For each transport, load its passengers using a separate connection.
            foreach (var transport in transports)
            {
                transport.Passengers = GetPassengersForTransport(transport.Id);
            }

            return transports;
        }

        // Helper method to retrieve passengers for a given PlannedTransport.
        private List<Passenger> GetPassengersForTransport(int transportId)
        {
            var passengers = new List<Passenger>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
SELECT 
    p.Id,
    pat.Id AS PatId,
    pat.Name AS PatName
FROM Passengers p
INNER JOIN Patients pat ON p.PatientId = pat.Id
WHERE p.PlannedTransportId = @TransportId";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TransportId", transportId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var passenger = new Passenger
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Patient = new Patient
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("PatId")),
                                    Name = reader.GetString(reader.GetOrdinal("PatName"))
                                }
                            };
                            passengers.Add(passenger);
                        }
                    }
                }
            }
            return passengers;
        }
    }
}
