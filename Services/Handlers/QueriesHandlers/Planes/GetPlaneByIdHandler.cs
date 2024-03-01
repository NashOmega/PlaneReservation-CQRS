using Core.Entities;
using Core.Interfaces.Repository;
using Core.Queries.Planes;
using MediatR;

namespace Services.Handlers.QueriesHandlers.Planes
{
    public class GetPlaneByIdHandler : IRequestHandler<GetPlaneByIdQuery, PlaneEntity?>
    {
        protected readonly IUnitOfWork _unitOfWork;

        public GetPlaneByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<PlaneEntity?> Handle(GetPlaneByIdQuery request, CancellationToken cancellationtoken)
        {
            var plane = await _unitOfWork.Planes.FindByIdAsync(request.PlaneId);
            return plane == null ? null : plane;
        }
    }
}
