using casus_ouderenzorg.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace casus_ouderenzorg.DAL
{
    public class MedicationDal
    {
        private readonly string _connectionString;
        public MedicationDal(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Medication> GetMedications()
        {
            var medications = new List<Medication>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
SELECT m.MedicationID, m.Name, m.Concentration, m.Form, m.StandardDosage, m.MedicationInteractionID,
       mi.Description, mi.Severity
FROM Medication m
LEFT JOIN MedicationInteraction mi ON m.MedicationInteractionID = mi.MedicationInteractionID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var interaction = new MedicationInteraction
                        {
                            MedicationInteractionID = reader.GetInt32(reader.GetOrdinal("MedicationInteractionID")),
                            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? string.Empty : reader.GetString(reader.GetOrdinal("Description")),
                            Severity = reader.IsDBNull(reader.GetOrdinal("Severity")) ? string.Empty : reader.GetString(reader.GetOrdinal("Severity"))
                        };

                        medications.Add(new Medication
                        {
                            MedicationID = reader.GetInt32(reader.GetOrdinal("MedicationID")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Concentration = reader.GetString(reader.GetOrdinal("Concentration")),
                            Form = reader.GetString(reader.GetOrdinal("Form")),
                            StandardDosage = reader.GetString(reader.GetOrdinal("StandardDosage")),
                            MedicationInteractionID = reader.GetInt32(reader.GetOrdinal("MedicationInteractionID")),
                            MedicationInteraction = interaction
                        });
                    }
                }
            }
            return medications;
        }
    }
}
