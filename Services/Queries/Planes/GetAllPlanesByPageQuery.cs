using Core.Response;
using MediatR;

namespace Core.Queries.Planes
{
    public class GetAllPlanesByPageQuery : IRequest<IEnumerable<PlaneResponse>>
    {
        public int Page {  get;}
        public int Size { get;}

        public GetAllPlanesByPageQuery(int page, int size)
        {
            Page = page;
            Size = size;   
        }
    }
}
