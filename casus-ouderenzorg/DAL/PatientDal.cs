using casus_ouderenzorg.Models;
using Microsoft.Data.SqlClient;
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
                string query = "SELECT Id, Name, CaregiverId FROM Patients WHERE CaregiverId = @CaregiverId";
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
                                CaregiverId = reader.IsDBNull(reader.GetOrdinal("CaregiverId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CaregiverId"))
                            });
                        }
                    }
                }
            }
            return patients;
        }
    }
}
