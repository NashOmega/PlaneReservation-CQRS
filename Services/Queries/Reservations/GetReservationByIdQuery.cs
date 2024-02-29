using Core.Response;
using MediatR;

namespace Core.Queries.Planes
{
    public class GetReservationByIdQuery : IRequest<ReservationResponse?>
    {
        public int ReservationId {  get;}

        public GetReservationByIdQuery(int reservationId)
        {
            ReservationId = reservationId;
        }
    }
}
