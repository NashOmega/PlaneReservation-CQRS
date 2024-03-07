using Hangfire;
using Services.Jobs.Interfaces;

namespace Api.Configuration.Jobs
{
    public class JobChecker
    {
        private readonly IVerificationService _verificationService;
        private readonly IRecurringJobManager _recurringJobManager;

        public JobChecker(IVerificationService verificationService, IRecurringJobManager recurringJobManager)
        {
            _verificationService = verificationService;
            _recurringJobManager = recurringJobManager;
        }

        public void CheckMonthlyJobExecution(string jobId)
        {
            if (_verificationService == null || _recurringJobManager == null)
            {
                throw new InvalidOperationException("Service has not been initialized. Call Initialize() first.");
            }

            var hasExecutedThisMonth = _verificationService.HasJobExecutedThisMonth(jobId);

            if (!hasExecutedThisMonth)
            {
                _recurringJobManager.Trigger(jobId);
            }
        }

    }
}
