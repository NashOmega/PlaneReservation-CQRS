using Core.Entities;
using Core.Request;
using Core.Response;
using MediatR;

namespace Services.Commands.Reservations
{
    public class AddReservationRequest : IRequest<ReservationResponse?>
    {
        public ReservationRequest ReservationRequest { get; }
        public PlaneEntity Plane { get; }

        public AddReservationRequest(ReservationRequest reservationRequest, PlaneEntity plane)
        {
            ReservationRequest = reservationRequest;
            Plane = plane; 
        }
    }
}
