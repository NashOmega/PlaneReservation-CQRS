using Core.Response;
using MediatR;

namespace Core.Queries.Planes
{
    public class GetAvailablesPlanesQuery : IRequest<IEnumerable<PlaneResponse>>
    {

    }
}
