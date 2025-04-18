using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using casus_ouderenzorg.Models;

namespace casus_ouderenzorg.DAL
{
    public class PatientDal
    {
        private readonly string _connectionString;
        public PatientDal(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Patient> GetAllPatients()
        {
            var patients = new List<Patient>();
            const string sql = @"
SELECT
    PatientID,
    Name
FROM Patient
ORDER BY Name";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                patients.Add(new Patient
                {
                    PatientID = reader.GetInt32(0),
                    Name = reader.GetString(1)
                });
            }
            return patients;
        }

        public List<Patient> GetPatientsByCaregiver(int caregiverId)
        {
            var patients = new List<Patient>();
            const string sql = @"
SELECT
    p.PatientID,
    p.Name,
    p.DateOfBirth,
    p.LocationID,
    l.Name AS LocationName
FROM Patient p
LEFT JOIN Location l ON p.LocationID = l.LocationID
WHERE p.CaregiverID = @caregiverId
ORDER BY p.Name";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add(new SqlParameter("@caregiverId", SqlDbType.Int) { Value = caregiverId });

            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                patients.Add(new Patient
                {
                    PatientID = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    DateOfBirth = reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(2),
                    LocationID = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                    Location = reader.IsDBNull(3)
                        ? null
                        : new Location { LocationID = reader.GetInt32(3), Name = reader.GetString(4) }
                });
            }

            return patients;
        }

        public Patient GetPatientById(int id)
        {
            const string sql = @"
SELECT
    p.PatientID,
    p.Name,
    p.DateOfBirth,
    p.LocationID,
    l.Name AS LocationName,
    p.BackgroundInfo
FROM Patient p
LEFT JOIN Location l ON p.LocationID = l.LocationID
WHERE p.PatientID = @id";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = id });

            conn.Open();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Patient
                {
                    PatientID = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    DateOfBirth = reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(2),
                    LocationID = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                    Location = reader.IsDBNull(3)
                        ? null
                        : new Location { LocationID = reader.GetInt32(3), Name = reader.GetString(4) },
                    BackgroundInfo = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
                };
            }
            return null;
        }
    }
}