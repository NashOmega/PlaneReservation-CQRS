using Services.Jobs.Interfaces;

namespace Services.Jobs
{
    public class MerchService : IMerchService
    {
        public void CreateMerch(int id)
        {
            Console.WriteLine($"This will create Merch for the driver ${id}");

        }

        public void DeleteMerch(int id)
        {
            Console.WriteLine($"This will remove Merch for the driver ${id}");
        }
    }
}
