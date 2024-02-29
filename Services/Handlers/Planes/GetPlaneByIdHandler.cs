
using AutoMapper;
using Core.Interfaces.Repository;
using Core.Queries.Planes;
using Core.Response;
using MediatR;

namespace Core.handlers.planes
{
    public class GetPlaneByIdHandler : IRequestHandler<GetPlaneByIdQuery, PlaneResponse?>
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public GetPlaneByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PlaneResponse?> Handle(GetPlaneByIdQuery request, CancellationToken cancellationtoken)
        {
            var plane = await _unitOfWork.Planes.FindByIdAsync(request.PlaneId);
            return plane == null ? null : _mapper.Map<PlaneResponse>(plane);
        }
    }
}
