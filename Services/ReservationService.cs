using AutoMapper;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Queries.Planes;
using Core.Request;
using Core.Response;
using MediatR;
using Microsoft.Extensions.Logging;
using Services.Commands.Reservations;

namespace Services
{
    public class ReservationService : ServiceBase<ReservationService>, IReservationService
    {

        private readonly IMediator _mediator;


        /// <summary>
        /// Initializes a new instance of the <see cref="ReservationService"/> class.
        /// </summary>
        /// <param name="reservationRepository">The repository for handling reservation data.</param>
        /// <param name="passengerRepository">The repository for handling passenger data.</param>
        /// <param name="planeRepository">The repository for handling plane data.</param>
        /// <param name="passengerService">The service for handling passenger-related operations.</param>
        /// <param name="mapper">The mapper for mapping between different types.</param>
        /// <param name="logger">The logger for logging messages.</param>
        public ReservationService(IMapper mapper, ILoggerFactory factory, IMediator mediator)
            : base(mapper, factory)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new reservation.
        /// </summary>
        /// <param name="reservationRequest">The request containing reservation details.</param>
        /// <returns>
        /// A MainResponse containing information about the created reservation if the creation operation was successful; 
        /// otherwise, a MainResponse with the appropriate error message.
        /// </returns>
        public async Task<MainResponse<ReservationResponse>> CreateReservation(ReservationRequest reservationRequest)
        {
            _logger.LogInformation("Creating Reseervation");

            MainResponse<ReservationResponse> res = new();
            try
            {
                var query = new GetPlaneByIdQuery(reservationRequest.PlaneId);
                var plane = await _mediator.Send(query);
                
                if (plane != null && IsPlaneAvailable(plane))
                {
                    if (IsPlaneSeatsSufficients(reservationRequest, plane))
                    { 
                        res = await AddReservation(reservationRequest, plane);
                    }
                    else
                    {
                        if (IsSameLastName(reservationRequest.PassengerRequests))
                        {
                            res.Message = "The Plane is Full";
                        }
                        else
                        {
                            var availableSeats = plane.AvailableSeats;
                            reservationRequest.PassengerRequests = ReducedPassengersList(reservationRequest.PassengerRequests, availableSeats);
                            res = await AddReservation(reservationRequest, plane);
                            if (res.Success) res.Message = "Insuffiscient Places. Just " + availableSeats + " of you found seat";
                        }
                    }
                }
                else
                {
                    res.Message = "Plane is Full";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error Occured: {ErrorMessage}", ex.Message);
                res.Message = ex.Message;
            }
            return res;
        }

        /// <summary>
        /// Adds a list of passengers to a reservation entity.
        /// </summary>
        /// <param name="reservation">The reservation entity to which passengers will be added.</param>
        /// <param name="passengerRequests">The list of passenger requests to be added to the reservation.</param>
        /// <returns>
        /// The reservation entity with passengers added if the adding operation was successful; 
        /// otherwise, a MainResponse with the appropriate error message.
        /// </returns>
        public async Task<MainResponse<ReservationResponse>> AddReservation(ReservationRequest reservationRequest, PlaneEntity plane)
        {
            _logger.LogInformation("Adding Reservation");
            MainResponse<ReservationResponse> res = new();
            var message = "Same Reservation Already Exists";
            try
            {
                var request = new AddReservationRequest(reservationRequest, plane);
                var reservationResponse = await _mediator.Send(request);
                if (reservationResponse != null)
                {
                    res.Success = true;
                    res.Data = reservationResponse;
                    message = "Reservation Created Successfully";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error Occured: {ErrorMessage}", ex.Message);
                message = ex.Message;
            }
            res.Message = message;
            return res;
        }

        /// <summary>
        /// Checks if a plane is available (i.e., has available seats).
        /// </summary>
        /// <param name="plane">The plane entity to check.</param>
        /// <returns>True if the plane is available; otherwise, false.</returns>
        public bool IsPlaneAvailable(PlaneEntity plane)
        {
            return plane.AvailableSeats > 0;
        }

        /// <summary>
        /// Checks if the number of available seats in a plane is sufficient for the given reservation request.
        /// </summary>
        /// <param name="reservationRequest">The reservation request containing passenger information.</param>
        /// <param name="plane">The plane entity to check available seats against.</param>
        /// <returns>
        /// True if the plane has enough available seats to accommodate all passengers in the reservation request,
        /// otherwise false.
        /// </returns>
        public bool IsPlaneSeatsSufficients(ReservationRequest reservationRequest, PlaneEntity plane)
        {
            return plane.AvailableSeats >= reservationRequest.PassengerRequests.Count;
        }

        /// <summary>
        /// Checks if all passengers in a reservation request have the same last name.
        /// </summary>
        /// <param name="passengerRequests">The list of passenger requests to check.</param>
        /// <returns>True if all passengers have the same last name; otherwise, false.</returns>
        public bool IsSameLastName(ICollection<PassengerRequest> passengerRequests)
        {
            return passengerRequests.Select(p => p.LastName).Distinct().Count() == 1;
        }

        /// <summary>
        /// Reduces the list of passenger requests to a specified number.
        /// </summary>
        /// <param name="passengerRequests">The list of passenger requests to reduce.</param>
        /// <param name="reducer">The number of passengers to keep in the list.</param>
        /// <returns>The reduced list of passenger requests.</returns>
        public ICollection<PassengerRequest> ReducedPassengersList(ICollection<PassengerRequest> passengerRequests, int reducer)
        {
            return passengerRequests.Take(reducer).ToList();
        }


        public async Task<MainResponse<ReservationResponse>> GetReservationById(int id)
        {
            var res = new MainResponse<ReservationResponse>();
            var message = "Plane Not Found";

            try
            {
                var query = new GetReservationByIdQuery(id);
                var reservationResponse = await _mediator.Send(query);
                if (reservationResponse != null)
                {
                    res.Data = reservationResponse;
                    res.Success = true;
                    message = "This is the reservation of id " + id;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error Occured: {ErrorMessage}", ex.Message);
                message = ex.Message;
            }
            res.Message = message;
            return res;
        }
    }
}
