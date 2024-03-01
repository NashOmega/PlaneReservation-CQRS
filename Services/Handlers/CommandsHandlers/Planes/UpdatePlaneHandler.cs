using AutoMapper;
using Core.Interfaces.Repository;
using Core.Response;
using MediatR;
using Services.Commands.Planes;

namespace Services.Handlers.CommandsHandlers.Planes
{
    public class UpdatePlaneHandler : IRequestHandler<UpdatePlaneRequest, PlaneResponse?>
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public UpdatePlaneHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PlaneResponse?> Handle(UpdatePlaneRequest request , CancellationToken cancellationToken)
        {
            var dbPlane = await _unitOfWork.Planes.FindByIdAsync(request.PlaneId);

            if (dbPlane == null) return null;

            var updatedPlane = await _unitOfWork.Planes.UpdateAsync(_mapper.Map(request.PlaneRequest, dbPlane));
            await _unitOfWork.CompleteAsync();
           
            return _mapper.Map<PlaneResponse>(updatedPlane);     
        }
    }
}
