using MediatR;

namespace Services.Commands.Planes
{

    public class DeletePlaneRequest : IRequest<bool>
    {
        public int PlaneId { get; }
        
        public DeletePlaneRequest(int planeId)
        {
            PlaneId = planeId;
        }
    }
}
