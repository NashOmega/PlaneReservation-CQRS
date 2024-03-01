using Core.Interfaces.Repository;
using MediatR;
using Services.Queries.Planes;

namespace Services.Handlers.QueriesHandlers.Planes
{
    public class PlaneExistenceHandler : IRequestHandler<PlaneExistenceQuery, bool>
    {
        protected readonly IUnitOfWork _unitOfWork;

        public PlaneExistenceHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(PlaneExistenceQuery request, CancellationToken cancellationToken)
        {
            var planesMatchingCriteria = await _unitOfWork.Planes.FindByConditionAsync(
            p => p.Name == request.PlaneRequest.Name && p.Model == request.PlaneRequest.Model && p.Serial == request.PlaneRequest.Serial);

            return planesMatchingCriteria.Any();
        }
    }
}
