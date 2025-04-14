using casus_ouderenzorg.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using Task = casus_ouderenzorg.Models.Task;

namespace casus_ouderenzorg.DAL
{
    public class TaskDal
    {
        private readonly string _connectionString;
        public TaskDal(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Task> GetTasksForCaregiver(int caregiverId)
        {
            var tasks = new List<Task>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
SELECT 
    t.Id,
    t.Description,
    t.Location,
    t.StartTime,
    t.EndTime,
    dp.[Date]
FROM dbo.Task t
INNER JOIN dbo.DayPlanning dp ON t.DayPlanningId = dp.Id
WHERE dp.CaregiverId = @CaregiverId
ORDER BY dp.[Date], t.StartTime";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CaregiverId", caregiverId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var task = new Task
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                Location = reader.IsDBNull(reader.GetOrdinal("Location"))
                                    ? string.Empty
                                    : reader.GetString(reader.GetOrdinal("Location")),
                                StartTime = reader.GetTimeSpan(reader.GetOrdinal("StartTime")),
                                EndTime = reader.GetTimeSpan(reader.GetOrdinal("EndTime")),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date"))
                            };
                            tasks.Add(task);
                        }
                    }
                }
            }
            return tasks;
        }
    }
}
