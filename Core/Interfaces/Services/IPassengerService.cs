using Core.Entities;
using Core.Request;
using Core.Response;

namespace Core.Interfaces.Services
{
    public interface IPassengerService
    {
        Task<MainResponse<ICollection<PassengerEntity>>> AddOrUpadatePassengers(ICollection<PassengerRequest> passengerRequests);
    }
}
