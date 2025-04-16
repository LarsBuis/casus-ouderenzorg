using casus_ouderenzorg.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace casus_ouderenzorg.DAL
{
    public class PatientDal
    {
        private readonly string _connectionString;
        public PatientDal(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Patient> GetPatientsByCaregiverId(int caregiverId)
        {
            var patients = new List<Patient>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
SELECT Id, 
       Name, 
       BackgroundInfo, 
       Traits, 
       DateOfBirth, 
       Address, 
       ContactPersonId, 
       CaregiverId 
FROM Patients 
WHERE CaregiverId = @CaregiverId";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CaregiverId", caregiverId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            patients.Add(new Patient
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                BackgroundInfo = reader.IsDBNull(reader.GetOrdinal("BackgroundInfo"))
                                    ? string.Empty
                                    : reader.GetString(reader.GetOrdinal("BackgroundInfo")),
                                Traits = reader.IsDBNull(reader.GetOrdinal("Traits"))
                                    ? string.Empty
                                    : reader.GetString(reader.GetOrdinal("Traits")),
                                DateOfBirth = reader.IsDBNull(reader.GetOrdinal("DateOfBirth"))
                                    ? (DateTime?)null
                                    : reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                ContactPersonId = reader.IsDBNull(reader.GetOrdinal("ContactPersonId"))
                                    ? (int?)null
                                    : reader.GetInt32(reader.GetOrdinal("ContactPersonId")),
                                CaregiverId = reader.IsDBNull(reader.GetOrdinal("CaregiverId"))
                                    ? (int?)null
                                    : reader.GetInt32(reader.GetOrdinal("CaregiverId"))
                            });
                        }
                    }
                }
            }
            return patients;
        }

        public Patient GetPatientById(int id)
        {
            Patient patient = null;
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
SELECT Id, 
       Name, 
       BackgroundInfo, 
       Traits, 
       DateOfBirth, 
       Address, 
       ContactPersonId, 
       CaregiverId 
FROM Patients 
WHERE Id = @Id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            patient = new Patient
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                BackgroundInfo = reader.IsDBNull(reader.GetOrdinal("BackgroundInfo"))
                                    ? string.Empty
                                    : reader.GetString(reader.GetOrdinal("BackgroundInfo")),
                                Traits = reader.IsDBNull(reader.GetOrdinal("Traits"))
                                    ? string.Empty
                                    : reader.GetString(reader.GetOrdinal("Traits")),
                                DateOfBirth = reader.IsDBNull(reader.GetOrdinal("DateOfBirth"))
                                    ? (DateTime?)null
                                    : reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                ContactPersonId = reader.IsDBNull(reader.GetOrdinal("ContactPersonId"))
                                    ? (int?)null
                                    : reader.GetInt32(reader.GetOrdinal("ContactPersonId")),
                                CaregiverId = reader.IsDBNull(reader.GetOrdinal("CaregiverId"))
                                    ? (int?)null
                                    : reader.GetInt32(reader.GetOrdinal("CaregiverId"))
                            };
                        }
                    }
                }
            }
            return patient;
        }
    }
}
