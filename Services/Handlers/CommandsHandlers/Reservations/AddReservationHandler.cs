using AutoMapper;
using Core.Entities;
using Core.Interfaces.Repository;
using Core.Response;
using Hangfire;
using MediatR;
using Services.Commands.Reservations;
using Services.Jobs.Interfaces;

namespace Services.Handlers.CommandsHandlers.Reservations
{
    public class AddReservationHandler : IRequestHandler<AddReservationRequest, ReservationResponse?>
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;
        protected readonly IMediator _mediator;

        public AddReservationHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<ReservationResponse?> Handle(AddReservationRequest request, CancellationToken cancellationToken)
        {
            var reservation = await FilterPassengersList(_mapper.Map<ReservationEntity>(request.ReservationRequest));

            if (reservation.Passengers.Count != 0)
            {
                reservation.Plane = request.Plane;
                var createdReservation = await _unitOfWork.Reservations.CreateAsync(reservation);
                await _unitOfWork.CompleteAsync();

                await AffectSeats(createdReservation, request.Plane);
                await _unitOfWork.CompleteAsync();

                return _mapper.Map<ReservationResponse>(createdReservation);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Adds the list of passengers to the reservation entity.
        /// </summary>
        /// <param name="reservation">The reservation entity to which passengers will be added.</param>
        /// <param name="passengerRequests">The collection of passenger requests containing information about passengers to be added.</param>
        /// <returns>The updated reservation entity with added passengers.</returns>
        public async Task<ReservationEntity> FilterPassengersList(ReservationEntity reservation)
        {
            List<PassengerEntity> passengersToRemove = new List<PassengerEntity>();

            foreach (PassengerEntity passenger in reservation.Passengers)
            {
                var dbPassenger = await _unitOfWork.Passengers.FindByEmail(passenger.Email);
                if (dbPassenger != null && DoesPassengerHaveTheSameReservation(dbPassenger, reservation))
                    passengersToRemove.Add(passenger);
            }
            foreach (PassengerEntity passenger in passengersToRemove)
            {
                reservation.Passengers.Remove(passenger);
            }
            return reservation;
        }

        /// <summary>
        /// Checks if a passenger already has the same reservation.
        /// </summary>
        /// <param name="passenger">The passenger entity to check.</param>
        /// <param name="reservation">The reservation entity to compare with.</param>
        /// <returns>True if the passenger already has the same reservation 
        /// based on the date and the city; otherwise, false.
        /// </returns>
        public bool DoesPassengerHaveTheSameReservation(PassengerEntity passenger, ReservationEntity reservation)
        {
            foreach (ReservationEntity oldReservation in passenger.Reservations)
            {
                if ((oldReservation.DepartureDate == reservation.DepartureDate
                    && oldReservation.DepartureCity == reservation.DepartureCity)
                    || oldReservation.PlaneId == reservation.PlaneId) return true;
            }
            return false;
        }

        public async Task AffectSeats(ReservationEntity reservation, PlaneEntity plane)
        {
            List<SeatArrangementEntity> seats = await _unitOfWork.Seats.FindSuitableAvailableSeats(reservation.Passengers.Count, plane.Id);
            var i = 0;
            foreach (var passenger in reservation.Passengers)
            {
                var seat = seats[i];
                passenger.LastSeatNumber = seat.SeatNumber;
                await _unitOfWork.Passengers.UpdateAsync(passenger);

                seat.Status = false;
                seat.Reservation = reservation;
                seat.Passenger = passenger;
                await _unitOfWork.Seats.UpdateAsync(seat);
                i++;
            }

            plane.AvailableSeats -= reservation.Passengers.Count;
            await _unitOfWork.Planes.UpdateAsync(plane);
        }
    }
}
