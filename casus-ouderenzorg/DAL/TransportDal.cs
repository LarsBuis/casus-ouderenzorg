using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using casus_ouderenzorg.Models;

namespace casus_ouderenzorg.Dal
{
    public class TransportDal
    {
        private readonly string _connectionString;

        public TransportDal(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<PlannedTransport> GetPlannedTransports()
        {
            var transports = new List<PlannedTransport>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"SELECT TransportId, RitNumber, DepartureTime, DepartureAddress, ArrivalTime, ArrivalAddress, Vehicle, Driver 
                                 FROM PlannedTransports";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var transport = new PlannedTransport
                            {
                                TransportId = reader.GetInt32(reader.GetOrdinal("TransportId")),
                                RitNumber = reader.GetString(reader.GetOrdinal("RitNumber")),
                                DepartureTime = reader.GetDateTime(reader.GetOrdinal("DepartureTime")),
                                DepartureAddress = reader.GetString(reader.GetOrdinal("DepartureAddress")),
                                ArrivalTime = reader.GetDateTime(reader.GetOrdinal("ArrivalTime")),
                                ArrivalAddress = reader.GetString(reader.GetOrdinal("ArrivalAddress")),
                                Vehicle = reader.GetString(reader.GetOrdinal("Vehicle")),
                                Driver = reader.GetString(reader.GetOrdinal("Driver"))
                            };
                            transports.Add(transport);
                        }
                    }
                }
            }
            return transports;
        }

        public PlannedTransport GetPlannedTransportById(int transportId)
        {
            PlannedTransport transport = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"SELECT TransportId, RitNumber, DepartureTime, DepartureAddress, ArrivalTime, ArrivalAddress, Vehicle, Driver 
                                 FROM PlannedTransports WHERE TransportId = @TransportId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TransportId", transportId);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            transport = new PlannedTransport
                            {
                                TransportId = reader.GetInt32(reader.GetOrdinal("TransportId")),
                                RitNumber = reader.GetString(reader.GetOrdinal("RitNumber")),
                                DepartureTime = reader.GetDateTime(reader.GetOrdinal("DepartureTime")),
                                DepartureAddress = reader.GetString(reader.GetOrdinal("DepartureAddress")),
                                ArrivalTime = reader.GetDateTime(reader.GetOrdinal("ArrivalTime")),
                                ArrivalAddress = reader.GetString(reader.GetOrdinal("ArrivalAddress")),
                                Vehicle = reader.GetString(reader.GetOrdinal("Vehicle")),
                                Driver = reader.GetString(reader.GetOrdinal("Driver"))
                            };
                        }
                    }
                }
            }

            if (transport != null)
            {
                // Retrieve passengers for this transport
                transport.Passengers = new List<Passenger>();

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string queryPassengers = @"SELECT PassengerId, Name 
                                               FROM Passengers 
                                               WHERE PlannedTransportId = @TransportId";
                    using (SqlCommand cmd = new SqlCommand(queryPassengers, conn))
                    {
                        cmd.Parameters.AddWithValue("@TransportId", transportId);
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var passenger = new Passenger
                                {
                                    PassengerId = reader.GetInt32(reader.GetOrdinal("PassengerId")),
                                    Name = reader.GetString(reader.GetOrdinal("Name"))
                                };
                                transport.Passengers.Add(passenger);
                            }
                        }
                    }
                }
            }

            return transport;
        }

        public void AddPlannedTransport(PlannedTransport transport)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Insert the transport and retrieve the new TransportId
                        string query = @"
                    INSERT INTO PlannedTransports 
                        (RitNumber, DepartureTime, DepartureAddress, ArrivalTime, ArrivalAddress, Vehicle, Driver)
                    VALUES 
                        (@RitNumber, @DepartureTime, @DepartureAddress, @ArrivalTime, @ArrivalAddress, @Vehicle, @Driver);
                    SELECT CAST(SCOPE_IDENTITY() AS int);";

                        using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@RitNumber", transport.RitNumber);
                            cmd.Parameters.AddWithValue("@DepartureTime", transport.DepartureTime);
                            cmd.Parameters.AddWithValue("@DepartureAddress", transport.DepartureAddress);
                            cmd.Parameters.AddWithValue("@ArrivalTime", transport.ArrivalTime);
                            cmd.Parameters.AddWithValue("@ArrivalAddress", transport.ArrivalAddress);
                            cmd.Parameters.AddWithValue("@Vehicle", transport.Vehicle);
                            cmd.Parameters.AddWithValue("@Driver", transport.Driver);

                            int newTransportId = (int)cmd.ExecuteScalar();

                            // Insert each passenger
                            if (transport.Passengers != null)
                            {
                                foreach (var passenger in transport.Passengers)
                                {
                                    string queryPassenger = @"
                                INSERT INTO Passengers (PlannedTransportId, Name)
                                VALUES (@PlannedTransportId, @Name)";
                                    using (SqlCommand cmdPassenger = new SqlCommand(queryPassenger, conn, transaction))
                                    {
                                        cmdPassenger.Parameters.AddWithValue("@PlannedTransportId", newTransportId);
                                        cmdPassenger.Parameters.AddWithValue("@Name", passenger.Name);
                                        cmdPassenger.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

    }
}
