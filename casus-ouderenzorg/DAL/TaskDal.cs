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
    t.IsCompleted,
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
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                IsCompleted = reader.GetBoolean(reader.GetOrdinal("IsCompleted"))
                            };
                            tasks.Add(task);
                        }
                    }
                }
            }
            return tasks;
        }
        // Retrieves tasks for a specific date.
        public List<Task> GetTasksByDate(DateTime selectedDate)
        {
            var tasks = new List<Task>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
SELECT 
    t.Id,
    dp.Date, 
    t.StartTime,
    t.EndTime,
    t.Location,
    t.Description,
    t.IsCompleted
FROM Task t
INNER JOIN DayPlanning dp ON t.DayPlanningId = dp.Id
WHERE CAST(dp.Date AS DATE) = @SelectedDate";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SelectedDate", selectedDate.Date);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tasks.Add(new Task
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                // Read the Date from the DayPlanning table.
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                StartTime = reader.GetTimeSpan(reader.GetOrdinal("StartTime")),
                                EndTime = reader.GetTimeSpan(reader.GetOrdinal("EndTime")),
                                Location = reader.IsDBNull(reader.GetOrdinal("Location"))
                                                ? string.Empty
                                                : reader.GetString(reader.GetOrdinal("Location")),
                                Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                                                ? string.Empty
                                                : reader.GetString(reader.GetOrdinal("Description")),
                                IsCompleted = reader.GetBoolean(reader.GetOrdinal("IsCompleted"))
                            });
                        }
                    }
                }
            }
            return tasks;
        }



        // Updates the completion status of a task.
        public void UpdateTaskCompletion(int taskId, bool isCompleted)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "UPDATE Task SET IsCompleted = @IsCompleted WHERE Id = @TaskId";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IsCompleted", isCompleted);
                    cmd.Parameters.AddWithValue("@TaskId", taskId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
