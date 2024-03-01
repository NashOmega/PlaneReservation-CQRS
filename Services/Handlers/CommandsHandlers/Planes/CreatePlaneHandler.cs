using AutoMapper;
using Core.Entities;
using Core.Interfaces.Repository;
using Core.Response;
using MediatR;
using Services.Commands.Planes;

namespace Services.Handlers.CommandsHandlers.Planes
{
    public class CreatePlaneHandler : IRequestHandler<CreatePlaneRequest, PlaneResponse>
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public CreatePlaneHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PlaneResponse> Handle(CreatePlaneRequest request , CancellationToken cancellationToken)
        {
            var createdPlane = await _unitOfWork.Planes.CreateAsync(_mapper.Map<PlaneEntity>(request.PlaneRequest));
            await _unitOfWork.Seats.GeneratePlaneSeats(createdPlane);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<PlaneResponse>(createdPlane);     
        }

      
    }
}
