using AutoMapper;
using Core.Entities;
using Core.Interfaces.Repository;
using Core.Queries.Passengers;
using Core.Request;
using MediatR;
using Services.Commands.Passengers;

namespace Services.Handlers.CommandsHandlers.Passengers
{
    public class AddOrUpdatePassengersHandler : IRequestHandler<AddOrUpdatePassengersRequest, List<PassengerEntity>>
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;
        protected readonly IMediator _mediator;

        public AddOrUpdatePassengersHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<List<PassengerEntity>> Handle (AddOrUpdatePassengersRequest request, CancellationToken cancellationToken)
        {
            var passengersList = new List<PassengerEntity>();
            foreach (PassengerRequest passengerRequest in request.PassengerRequests)
            {
                var query = new GetPassengerByEmailQuery(passengerRequest.Email);
                var dbPassenger = await _mediator.Send(query);
                if (dbPassenger == null)
                {
                    var addedPassenger = await _unitOfWork.Passengers.CreateAsync(_mapper.Map<PassengerEntity>(passengerRequest));
                    if (addedPassenger != null) passengersList.Add(addedPassenger);
                }
                else
                {
                    var updatePassenger = await _unitOfWork.Passengers.UpdateAsync(_mapper.Map(passengerRequest, dbPassenger));
                    if (updatePassenger != null) passengersList.Add(updatePassenger);
                }
            }
           await _unitOfWork.CompleteAsync();
            return passengersList;
        }
    }
}
