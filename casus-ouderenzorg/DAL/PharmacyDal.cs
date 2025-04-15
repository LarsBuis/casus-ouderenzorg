using casus_ouderenzorg.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace casus_ouderenzorg.DAL
{
    public class PharmacyDal
    {
        private readonly string _connectionString;
        public PharmacyDal(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Pharmacy> GetPharmacies()
        {
            var pharmacies = new List<Pharmacy>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT PharmacyID, Name, Address, PhoneNumber FROM Pharmacy";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pharmacies.Add(new Pharmacy
                        {
                            PharmacyID = reader.GetInt32(reader.GetOrdinal("PharmacyID")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Address = reader.GetString(reader.GetOrdinal("Address")),
                            PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber"))
                        });
                    }
                }
            }
            return pharmacies;
        }
    }
}
