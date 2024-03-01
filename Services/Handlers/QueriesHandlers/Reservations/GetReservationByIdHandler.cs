
using AutoMapper;
using Core.Interfaces.Repository;
using Core.Queries.Planes;
using Core.Response;
using MediatR;

namespace Services.Handlers.Queries.Reservations
{
    public class GetReservationByIdHandler : IRequestHandler<GetReservationByIdQuery, ReservationResponse?>
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public GetReservationByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ReservationResponse?> Handle(GetReservationByIdQuery request, CancellationToken cancellationtoken)
        {
            var reservation = await _unitOfWork.Reservations.FindByIdIncludePassengers(request.ReservationId);
            return reservation == null ? null : _mapper.Map<ReservationResponse>(reservation);
        }
    }
}
