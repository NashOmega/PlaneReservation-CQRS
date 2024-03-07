using Services.Jobs.Interfaces;

namespace Services.Jobs
{
    public  class MaintenanceService : IMaintenanceService
    {
        public void SyncRecords()
        {
            Console.WriteLine($"The sync has started");

        }
    }
}
