namespace Services.Jobs.Interfaces
{
    public interface IVerificationService
    {
        bool HasJobExecutedThisMonth(string jobId);
        bool HasJobExecutedToday(string jobId);
        bool HasJobExecutedThisHour(string jobId);
    }
}
