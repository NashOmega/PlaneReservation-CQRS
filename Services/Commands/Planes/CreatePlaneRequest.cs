using Core.Request;
using Core.Response;
using MediatR;

namespace Services.Commands.Planes
{
   
    public class CreatePlaneRequest : IRequest<PlaneResponse>
    {
        public PlaneRequest PlaneRequest { get; }

        public CreatePlaneRequest(PlaneRequest planeRequest)
        {
            PlaneRequest = planeRequest;
        }
    }
}
