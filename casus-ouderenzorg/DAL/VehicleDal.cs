using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using casus_ouderenzorg.Models;

namespace casus_ouderenzorg.DAL
{
    public class VehicleDal
    {
        private readonly string _connectionString;
        public VehicleDal(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Vehicle> GetAllVehicles()
        {
            var vehicles = new List<Vehicle>();
            const string sql = @"
SELECT
    VehicleID,
    Name
FROM Vehicle
ORDER BY Name";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                vehicles.Add(new Vehicle
                {
                    VehicleID = reader.GetInt32(0),
                    Name = reader.GetString(1)
                });
            }
            return vehicles;
        }
    }
}
