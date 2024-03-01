﻿using Core.Entities;
using Core.Request;
using Core.Response;

namespace Core.Interfaces.Services
{
    public interface IReservationService
    {
        Task<MainResponse<ReservationResponse>> CreateReservation(ReservationRequest reservationRequest);

        Task<MainResponse<ReservationResponse>> AddReservation(ReservationRequest reservationRequest, PlaneEntity plane);

        bool IsPlaneAvailable(PlaneEntity plane);

        bool IsPlaneSeatsSufficients(ReservationRequest reservationRequest, PlaneEntity plane);

        bool IsSameLastName(ICollection<PassengerRequest> passengerRequests);

        ICollection<PassengerRequest> ReducedPassengersList(ICollection<PassengerRequest> passengerRequests, int reducer);

        Task<MainResponse<ReservationResponse>> GetReservationById(int id);
    }
}
