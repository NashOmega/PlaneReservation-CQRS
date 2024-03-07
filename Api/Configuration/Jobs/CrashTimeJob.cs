using Hangfire;

namespace Api.Configuration.Jobs
{
    public static class CrashTimeJob
    {
        public static void RunNotExecutedJobs(this IApplicationBuilder app)
        {
            RecurringJob.AddOrUpdate<JobChecker>(
                "RunNotExecutedJobs",
                x => x.CheckMonthlyJobExecution("suppression_merch_job"),
                Cron.Daily
                );
        }
    }
}
