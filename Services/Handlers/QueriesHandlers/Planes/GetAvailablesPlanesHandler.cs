
using AutoMapper;
using Core.Interfaces.Repository;
using Core.Queries.Planes;
using Core.Response;
using MediatR;

namespace Services.Handlers.QueriesHandlers.Planes
{
    public class GetAvailablesPlanesHandler : IRequestHandler<GetAvailablesPlanesQuery, IEnumerable<PlaneResponse>>
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public GetAvailablesPlanesHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<PlaneResponse>> Handle(GetAvailablesPlanesQuery request, CancellationToken cancellationtoken)
        {
            var planes = await _unitOfWork.Planes.FindAvailablePlanesAsync();
            return planes.Select(p => _mapper.Map<PlaneResponse>(p)).ToList();
        }
    }
}
