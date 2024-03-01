using Core.Entities;
using Core.Request;
using MediatR;

namespace Services.Commands.Passengers
{
    public class AddOrUpdatePassengersRequest : IRequest<List<PassengerEntity>>
    {
        public ICollection<PassengerRequest> PassengerRequests { get;}

        public AddOrUpdatePassengersRequest(ICollection<PassengerRequest> passengerRequests)
        {
            PassengerRequests = passengerRequests;
        }
    }
}
