using Core.Entities;
using MediatR;

namespace Core.Queries.Passengers
{
    public class GetPassengerByEmailQuery : IRequest<PassengerEntity?>
    {
        public string Email {  get;}

        public GetPassengerByEmailQuery(string email)
        {
           Email = email;
        }
    }
}
