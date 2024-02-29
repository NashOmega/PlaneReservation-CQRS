using Core.Response;
using MediatR;

namespace Core.Queries.Planes
{
    public class GetPlaneByIdQuery : IRequest<PlaneResponse?>
    {
        public int PlaneId {  get;}

        public GetPlaneByIdQuery(int planeId)
        {  
            PlaneId = planeId;
        }
    }
}
