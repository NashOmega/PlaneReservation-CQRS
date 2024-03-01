using Core.Entities;
using MediatR;

namespace Core.Queries.Planes
{
    public class GetPlaneByIdQuery : IRequest<PlaneEntity?>
    {
        public int PlaneId {  get;}

        public GetPlaneByIdQuery(int planeId)
        {  
            PlaneId = planeId;
        }
    }
}
