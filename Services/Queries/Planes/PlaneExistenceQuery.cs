using Core.Request;
using MediatR;

namespace Services.Queries.Planes
{
    public class PlaneExistenceQuery : IRequest<bool>
    {
        public PlaneRequest PlaneRequest { get; }
        public PlaneExistenceQuery(PlaneRequest planeRequest)
        {
            PlaneRequest = planeRequest;
        }
    }
}
