
using AutoMapper;
using Core.Entities;
using Core.Interfaces.Repository;
using Core.Queries.Passengers;
using Core.Queries.Planes;
using Core.Response;
using MediatR;

namespace Services.Handlers.Queries.Reservations
{
    public class GetPassengerByEmailHandler : IRequestHandler<GetPassengerByEmailQuery, PassengerEntity?>
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public GetPassengerByEmailHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PassengerEntity?> Handle(GetPassengerByEmailQuery request, CancellationToken cancellationtoken)
        {
            var passenger = await _unitOfWork.Passengers.FindByEmail(request.Email);
            return passenger == null ? null : passenger;
        }
    }
}
