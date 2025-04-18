using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using casus_ouderenzorg.Models;

namespace casus_ouderenzorg.DAL
{
    public class OrderDal
    {
        private readonly string _connectionString;
        public OrderDal(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void CreateOrder(Order order)
        {
            const string sql = @"
INSERT INTO [Order] 
    (PatientID, OrderDate, Medication, Amount, Concentration, Status)
VALUES
    (@PatientID, @OrderDate, @Medication, @Amount, @Concentration, @Status)";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@PatientID", order.PatientID);
            cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
            cmd.Parameters.AddWithValue("@Medication", order.Medication);
            cmd.Parameters.AddWithValue("@Amount", (object?)order.Amount ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Concentration", (object?)order.Concentration ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", (object?)order.Status ?? DBNull.Value);

            conn.Open();
            cmd.ExecuteNonQuery();
        }



        public List<Order> GetOrdersByPatient(int patientId)
        {
            var orders = new List<Order>();
            const string sql = @"
SELECT
    o.OrderID,
    o.OrderDate,
    o.Medication,
    o.Amount,
    o.Concentration,
    o.Status
FROM [Order] o
WHERE o.PatientID = @patientId
ORDER BY o.OrderDate DESC";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add(new SqlParameter("@patientId", SqlDbType.Int) { Value = patientId });

            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                orders.Add(new Order
                {
                    OrderID = reader.GetInt32(0),
                    OrderDate = reader.GetDateTime(1),
                    Medication = reader.GetString(2),
                    Amount = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                    Concentration = reader.IsDBNull(4) ? null : reader.GetString(4),
                    Status = reader.IsDBNull(5) ? null : reader.GetString(5),
                });
            }
            return orders;
        }
    }
}