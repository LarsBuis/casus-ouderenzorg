using casus_ouderenzorg.Models;
using Microsoft.Data.SqlClient;
using System;

namespace casus_ouderenzorg.DAL
{
    public class MedicationOrderDal
    {
        private readonly string _connectionString;
        public MedicationOrderDal(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Inserts a new Order record into the database and returns the new OrderID.
        public int InsertOrder(Order order)
        {
            int newOrderId = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                // [Order] is enclosed in brackets because Order is a reserved word.
                string sql = @"
INSERT INTO [Order] (OrderDate, Status, CaregiverID, PatientID, PharmacyID)
VALUES (@OrderDate, @Status, @CaregiverID, @PatientID, @PharmacyID);
SELECT CAST(SCOPE_IDENTITY() AS int);";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    // Use parameters to prevent SQL injection.
                    cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                    cmd.Parameters.AddWithValue("@Status", order.Status);
                    cmd.Parameters.AddWithValue("@CaregiverID", order.CaregiverID);
                    cmd.Parameters.AddWithValue("@PatientID", order.PatientID);
                    cmd.Parameters.AddWithValue("@PharmacyID", order.PharmacyID);
                    newOrderId = (int)cmd.ExecuteScalar();
                }
            }
            return newOrderId;
        }

        // Inserts a new OrderLine record.
        public void InsertOrderLine(OrderLine orderLine)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = @"
INSERT INTO OrderLine (OrderID, MedicationID, Dosage, Frequency, TreatmentDuration)
VALUES (@OrderID, @MedicationID, @Dosage, @Frequency, @TreatmentDuration);";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@OrderID", orderLine.OrderID);
                    cmd.Parameters.AddWithValue("@MedicationID", orderLine.MedicationID);
                    cmd.Parameters.AddWithValue("@Dosage", orderLine.Dosage);
                    cmd.Parameters.AddWithValue("@Frequency", orderLine.Frequency);
                    cmd.Parameters.AddWithValue("@TreatmentDuration", orderLine.TreatmentDuration);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
