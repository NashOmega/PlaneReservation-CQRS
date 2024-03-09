using Hangfire;
using Hangfire.Storage;
using Services.Jobs.Interfaces;

namespace Services.Jobs
{
    public class VerificationService : IVerificationService
    {
        public bool HasJobExecutedThisMonth(string jobId)
        {
            var storage = JobStorage.Current;
            using (var connection = storage.GetConnection())
            {
                var jobData = connection.GetRecurringJobs().Find(j => j.Id == jobId);

                if (jobData == null)
                {
                    return false;
                }
                var lastExecutionDate = jobData.LastExecution;

                if (lastExecutionDate == null)
                {
                    return false;
                }
                var currentDate = DateTime.Now;
                return lastExecutionDate.Value.Month == currentDate.Month && lastExecutionDate.Value.Year == currentDate.Year;
            }
        }

        public bool HasJobExecutedToday(string jobId)
        {
            var storage = JobStorage.Current;
            using (var connection = storage.GetConnection())
            {
                var jobData = connection.GetRecurringJobs().Find(j => j.Id == jobId);

                if (jobData == null)
                {
                    return false;
                }
                var lastExecutionDate = jobData.LastExecution;

                if (lastExecutionDate == null)
                {
                    return false;
                }
                var currentDate = DateTime.Now;
                return lastExecutionDate.Value.Day == currentDate.Day
                    && lastExecutionDate.Value.Month == currentDate.Month
                    && lastExecutionDate.Value.Year == currentDate.Year;
            }
        }

        public bool HasJobExecutedThisHour(string jobId)
        {
            var storage = JobStorage.Current;
            using (var connection = storage.GetConnection())
            {
                var jobData = connection.GetRecurringJobs().Find(j => j.Id == jobId);

                if (jobData == null)
                {
                    return false;
                }
                var lastExecutionDate = jobData.LastExecution;

                if (lastExecutionDate == null)
                {
                    return false;
                }
                var currentDate = DateTime.Now;
                return lastExecutionDate.Value.Hour == currentDate.Hour 
                    && lastExecutionDate.Value.Day == currentDate.Day
                    && lastExecutionDate.Value.Month == currentDate.Month
                    && lastExecutionDate.Value.Year == currentDate.Year;
            }
        }
    }
}
