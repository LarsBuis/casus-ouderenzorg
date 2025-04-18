using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using casus_ouderenzorg.Models;

namespace casus_ouderenzorg.DAL
{
    public class DriverDal
    {
        private readonly string _connectionString;
        public DriverDal(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Driver> GetAllDrivers()
        {
            var drivers = new List<Driver>();
            const string sql = @"
SELECT
    DriverID,
    Name
FROM Driver
ORDER BY Name";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                drivers.Add(new Driver
                {
                    DriverID = reader.GetInt32(0),
                    Name = reader.GetString(1)
                });
            }
            return drivers;
        }
    }
}
