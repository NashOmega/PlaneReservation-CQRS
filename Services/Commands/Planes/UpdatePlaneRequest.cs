using Core.Request;
using Core.Response;
using MediatR;

namespace Services.Commands.Planes
{
   
    public class UpdatePlaneRequest : IRequest<PlaneResponse?>
    {
        public int PlaneId { get; }
        public PlaneRequest PlaneRequest { get; }
        
        public UpdatePlaneRequest(int planeId, PlaneRequest planeRequest)
        {
            PlaneRequest = planeRequest;
            PlaneId = planeId;
        }
    }
}
