using AutoMapper;
using Core.Interfaces.Repository;
using Core.Response;
using MediatR;
using Services.Commands.Planes;

namespace Services.Handlers.CommandsHandlers.Planes
{
    public class DeletePlaneHandler : IRequestHandler<DeletePlaneRequest, bool>
    {
        protected readonly IUnitOfWork _unitOfWork;

        public DeletePlaneHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeletePlaneRequest request , CancellationToken cancellationToken)
        {
            var dbPlane = await _unitOfWork.Planes.FindByIdAsync(request.PlaneId);

            if (dbPlane == null) return false;
            
            await _unitOfWork.Planes.DeleteAsync(dbPlane);
            await _unitOfWork.CompleteAsync();
            return true;  
        }
    }
}
