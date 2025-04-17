using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using casus_ouderenzorg.Models;
using Task = casus_ouderenzorg.Models.Task;

namespace casus_ouderenzorg.DAL
{
    public class TaskDal
    {
        private readonly string _connectionString;

        public TaskDal(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("Connection string 'ZorgDb' is not configured.");

            _connectionString = connectionString;
        }

        public List<Task> GetTasks()
        {
            var tasks = new List<Task>();
            const string sql = @"
SELECT
    t.TaskID,
    t.TaskName,
    t.TaskDate,
    t.StartTime,
    t.EndTime,
    t.Description,
    t.IsCompleted,
    t.CaregiverID,
    c.Name AS CaregiverName,
    t.LocationID,
    l.Name AS LocationName,
    t.PatientID,
    p.Name AS PatientName
FROM [Task] t
LEFT JOIN Caregiver c ON t.CaregiverID = c.CaregiverID
LEFT JOIN Location l  ON t.LocationID   = l.LocationID
LEFT JOIN Patient p   ON t.PatientID    = p.PatientID
WHERE t.CaregiverID = 1
ORDER BY t.TaskDate, t.StartTime;";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                tasks.Add(new Task
                {
                    TaskID = reader.GetInt32(0),
                    TaskName = reader.GetString(1),
                    TaskDate = reader.GetDateTime(2),
                    StartTime = reader.GetTimeSpan(3),
                    EndTime = reader.GetTimeSpan(4),
                    Description = reader.IsDBNull(5) ? null : reader.GetString(5),
                    IsCompleted = reader.GetBoolean(6),
                    CaregiverID = reader.IsDBNull(7) ? (int?)null : reader.GetInt32(7),
                    Caregiver = reader.IsDBNull(7) ? null : new Caregiver { CaregiverID = reader.GetInt32(7), Name = reader.GetString(8) },
                    LocationID = reader.IsDBNull(9) ? (int?)null : reader.GetInt32(9),
                    Location = reader.IsDBNull(9) ? null : new Location { LocationID = reader.GetInt32(9), Name = reader.GetString(10) },
                    PatientID = reader.IsDBNull(11) ? (int?)null : reader.GetInt32(11),
                    Patient = reader.IsDBNull(11) ? null : new Patient { PatientID = reader.GetInt32(11), Name = reader.GetString(12) }
                });
            }

            return tasks;
        }
    }
}