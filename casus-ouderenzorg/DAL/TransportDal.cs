using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using casus_ouderenzorg.Models;

namespace casus_ouderenzorg.DAL
{
    public class TransportDal
    {
        private readonly string _connectionString;
        public TransportDal(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<TransportView> GetTransportsByCaregiver(int caregiverId)
        {
            var list = new List<TransportView>();
            const string sql = @"
SELECT
    t.TransportID,
    t.TransportDate,
    t.StartTime,
    t.ReturnTime,
    t.Departure,
    t.Destination,
    t.Reason,
    p.Name AS PatientName,
    d.Name AS DriverName,
    v.Name AS VehicleName
FROM Transport t
LEFT JOIN Patient p ON t.PatientID = p.PatientID
LEFT JOIN Driver d  ON t.DriverID  = d.DriverID
LEFT JOIN Vehicle v ON t.VehicleID = v.VehicleID
WHERE t.CaregiverID = @caregiverId
ORDER BY t.TransportDate, t.StartTime";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add(new SqlParameter("@caregiverId", SqlDbType.Int) { Value = caregiverId });

            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new TransportView
                {
                    TransportID = reader.GetInt32(0),
                    TransportDate = reader.GetDateTime(1),
                    StartTime = reader.GetTimeSpan(2),
                    ReturnTime = reader.GetTimeSpan(3),
                    Departure = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    Destination = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    Reason = reader.IsDBNull(6) ? null : reader.GetString(6),
                    PatientName = reader.IsDBNull(7) ? null : reader.GetString(7),
                    DriverName = reader.IsDBNull(8) ? null : reader.GetString(8),
                    VehicleName = reader.IsDBNull(9) ? null : reader.GetString(9)
                });
            }
            return list;
        }
    }
}