using casus_ouderenzorg.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace casus_ouderenzorg.DAL
{
    public class OrderDal
    {
        private readonly string _connectionString;
        public OrderDal(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Order> GetOrdersByPatientId(int patientId)
        {
            var orders = new List<Order>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
SELECT 
    o.OrderID, 
    o.OrderDate, 
    o.Status, 
    o.CaregiverID, 
    o.PatientID, 
    o.PharmacyID,
    p.Name AS PharmacyName, 
    p.Address AS PharmacyAddress, 
    p.PhoneNumber AS PharmacyPhoneNumber
FROM [Order] o
LEFT JOIN Pharmacy p ON o.PharmacyID = p.PharmacyID
WHERE o.PatientID = @PatientID
ORDER BY o.OrderDate DESC";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PatientID", patientId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var order = new Order
                            {
                                OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                                OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                                Status = reader.GetString(reader.GetOrdinal("Status")),
                                CaregiverID = reader.GetInt32(reader.GetOrdinal("CaregiverID")),
                                PatientID = reader.GetInt32(reader.GetOrdinal("PatientID")),
                                PharmacyID = reader.GetInt32(reader.GetOrdinal("PharmacyID")),
                                Pharmacy = new Pharmacy
                                {
                                    PharmacyID = reader.GetInt32(reader.GetOrdinal("PharmacyID")),
                                    Name = reader.IsDBNull(reader.GetOrdinal("PharmacyName")) ? string.Empty : reader.GetString(reader.GetOrdinal("PharmacyName")),
                                    Address = reader.IsDBNull(reader.GetOrdinal("PharmacyAddress")) ? string.Empty : reader.GetString(reader.GetOrdinal("PharmacyAddress")),
                                    PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PharmacyPhoneNumber")) ? string.Empty : reader.GetString(reader.GetOrdinal("PharmacyPhoneNumber"))
                                }
                            };
                            orders.Add(order);
                        }
                    }
                }

                // For each order, load its order lines.
                foreach (var order in orders)
                {
                    string query2 = @"
SELECT 
    ol.OrderLineID, 
    ol.OrderID, 
    ol.MedicationID, 
    ol.Dosage, 
    ol.Frequency, 
    ol.TreatmentDuration,
    m.Name AS MedicationName, 
    m.Concentration, 
    m.Form, 
    m.StandardDosage, 
    m.MedicationInteractionID,
    mi.Description, 
    mi.Severity
FROM OrderLine ol
LEFT JOIN Medication m ON ol.MedicationID = m.MedicationID
LEFT JOIN MedicationInteraction mi ON m.MedicationInteractionID = mi.MedicationInteractionID
WHERE ol.OrderID = @OrderID";
                    using (var cmd2 = new SqlCommand(query2, conn))
                    {
                        cmd2.Parameters.AddWithValue("@OrderID", order.OrderID);
                        using (var reader2 = cmd2.ExecuteReader())
                        {
                            var orderLines = new List<OrderLine>();
                            while (reader2.Read())
                            {
                                var medication = new Medication
                                {
                                    MedicationID = reader2.GetInt32(reader2.GetOrdinal("MedicationID")),
                                    Name = reader2.IsDBNull(reader2.GetOrdinal("MedicationName")) ? string.Empty : reader2.GetString(reader2.GetOrdinal("MedicationName")),
                                    Concentration = reader2.IsDBNull(reader2.GetOrdinal("Concentration")) ? string.Empty : reader2.GetString(reader2.GetOrdinal("Concentration")),
                                    Form = reader2.IsDBNull(reader2.GetOrdinal("Form")) ? string.Empty : reader2.GetString(reader2.GetOrdinal("Form")),
                                    StandardDosage = reader2.IsDBNull(reader2.GetOrdinal("StandardDosage")) ? string.Empty : reader2.GetString(reader2.GetOrdinal("StandardDosage")),
                                    MedicationInteractionID = reader2.IsDBNull(reader2.GetOrdinal("MedicationInteractionID")) ? 0 : reader2.GetInt32(reader2.GetOrdinal("MedicationInteractionID")),
                                    MedicationInteraction = new MedicationInteraction
                                    {
                                        MedicationInteractionID = reader2.IsDBNull(reader2.GetOrdinal("MedicationInteractionID")) ? 0 : reader2.GetInt32(reader2.GetOrdinal("MedicationInteractionID")),
                                        Description = reader2.IsDBNull(reader2.GetOrdinal("Description")) ? string.Empty : reader2.GetString(reader2.GetOrdinal("Description")),
                                        Severity = reader2.IsDBNull(reader2.GetOrdinal("Severity")) ? string.Empty : reader2.GetString(reader2.GetOrdinal("Severity"))
                                    }
                                };
                                var orderLine = new OrderLine
                                {
                                    OrderLineID = reader2.GetInt32(reader2.GetOrdinal("OrderLineID")),
                                    OrderID = reader2.GetInt32(reader2.GetOrdinal("OrderID")),
                                    MedicationID = reader2.GetInt32(reader2.GetOrdinal("MedicationID")),
                                    Dosage = reader2.IsDBNull(reader2.GetOrdinal("Dosage")) ? string.Empty : reader2.GetString(reader2.GetOrdinal("Dosage")),
                                    Frequency = reader2.IsDBNull(reader2.GetOrdinal("Frequency")) ? string.Empty : reader2.GetString(reader2.GetOrdinal("Frequency")),
                                    TreatmentDuration = reader2.IsDBNull(reader2.GetOrdinal("TreatmentDuration")) ? string.Empty : reader2.GetString(reader2.GetOrdinal("TreatmentDuration")),
                                    Medication = medication
                                };
                                orderLines.Add(orderLine);
                            }
                            order.OrderLines = orderLines;
                        }
                    }
                }
            }
            return orders;
        }
    }
}
