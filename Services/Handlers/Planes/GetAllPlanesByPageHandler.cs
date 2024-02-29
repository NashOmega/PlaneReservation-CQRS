using AutoMapper;
using Core.Interfaces.Repository;
using Core.Queries.Planes;
using Core.Response;
using MediatR;

namespace Core.handlers.planes
{
    public class GetAllPlanesByPageHandler : IRequestHandler<GetAllPlanesByPageQuery, IEnumerable<PlaneResponse>>
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public GetAllPlanesByPageHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<PlaneResponse>> Handle(GetAllPlanesByPageQuery request, CancellationToken cancellationtoken)
        {
            var planes = await _unitOfWork.Planes.FindAllPlanesByPageAsync(request.Page, request.Size);
            return planes.Select(p => _mapper.Map<PlaneResponse>(p)).ToList();
        }
    }
}
