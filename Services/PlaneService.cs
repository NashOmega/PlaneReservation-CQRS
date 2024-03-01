using AutoMapper;
using Core.Interfaces.Services;
using Core.Queries.Planes;
using Core.Request;
using Core.Response;
using MediatR;
using Microsoft.Extensions.Logging;
using Services.Commands.Planes;
using Services.Queries.Planes;

namespace Services
{
    public class PlaneService : ServiceBase<PlaneService>, IPlaneService
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneService"/> class.
        /// </summary>
        /// <param name="planeRepository">The repository for managing plane data.</param>
        /// <param name="mapper">The mapper for object mapping.</param>
        /// <param name="logger">The logger for logging messages.</param>
        public PlaneService(IMapper mapper, ILoggerFactory factory, IMediator mediator)
            : base(mapper, factory)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves a list of planes paginated by the specified page number and page size.
        /// </summary>
        /// <param name="page">The page number to retrieve.</param>
        /// <param name="size">The number of items per page.</param>
        /// <returns cref="MainResponse{IEnumerable{PlaneResponse}}">
        /// A MainResponse containing a paginated list of PlaneResponse objects if successful, 
        /// otherwise, a MainResponse with the appropriate error message.
        /// </returns>
        public async Task<MainResponse<IEnumerable<PlaneResponse>>> GetPlanesByPage(int page, int size)
        {
            MainResponse<IEnumerable<PlaneResponse>> res = new();
            var message = "This is the planes list";
            try
            {
                var query = new GetAllPlanesByPageQuery(page, size);
                res.Data = await _mediator.Send(query);
                res.Success = true; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{repo} An error Occured: {ErrorMessage}", typeof(PlaneService), ex.Message);
                message = ex.Message;
            }
            res.Message = message;
            return res;
        }

        /// <summary>
        /// Retrieves a list of availables planes paginated by the specified page number and page size.
        /// </summary>
        /// <param name="page">The page number to retrieve.</param>
        /// <param name="size">The number of items per page.</param>
        /// <returns cref="MainResponse{IEnumerable{PlaneResponse}}">
        /// A MainResponse containing a paginated list of PlaneResponse objects if successful, 
        /// otherwise, a MainResponse with the appropriate error message. 
        /// </returns>
        public async Task<MainResponse<IEnumerable<PlaneResponse>>> GetAvailablePlanes()
        {
            MainResponse<IEnumerable<PlaneResponse>> res = new();
            var message = "This is the available planes list";
            try
            {
                var query = new GetAvailablesPlanesQuery();
                res.Data = await _mediator.Send(query);
                res.Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{repo} An error Occured: {ErrorMessage}", typeof(PlaneService), ex.Message);
                message = ex.Message;
            }
            res.Message = message;
            return res;
        }

        /// <summary>
        /// Retrieves a plane by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the plane to retrieve.</param>
        /// <returns cref="MainResponse{PlaneResponse}">
        /// A MainResponse containing information about the plane if found, 
        /// otherwise, a MainResponse with the appropriate error message.
        /// </returns>
        public async Task<MainResponse<PlaneResponse>> GetPlaneById(int id)
        {
            MainResponse<PlaneResponse> res = new();
            var message = "Plane Not Found";
            try
            {
                var query = new GetPlaneByIdQuery(id);
                var plane = await _mediator.Send(query);
                if (plane != null)
                {
                    res.Data = _mapper.Map<PlaneResponse>(plane);
                    res.Success = true;
                    message = "This is the plane of id " + id;
                }  
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{repo} An error Occured: {ErrorMessage}", typeof(PlaneService), ex.Message);
                message = ex.Message;
            }
            res.Message = message;
            return res;
        }

        /// <summary>
        /// Adds a new plane to the database.
        /// </summary>
        /// <param name="planeRequest">The details of the plane to be added.</param>
        /// <returns cref="MainResponse{PlaneResponse}">
        /// A MainResponse containing information about the newly added plane if successful, 
        /// otherwise, a MainResponse with the appropriate error message.
        /// </returns>
        public async Task<MainResponse<PlaneResponse>> AddPlane(PlaneRequest planeRequest)
        {
            MainResponse<PlaneResponse> res = new();
            var message = "Plane Already Exists";
            try
            {
                var query = new PlaneExistenceQuery(planeRequest);
                var IsPlaneExists = await _mediator.Send(query);
                
                if(!IsPlaneExists)
                {
                    var request = new CreatePlaneRequest(planeRequest);
                    res.Data = await _mediator.Send(request);
                    res.Success = true;
                    message = "Plane Created Successfully";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{repo} An error Occured: {ErrorMessage}", typeof(PlaneService), ex.Message);
                message = ex.Message;
            }
            res.Message = message;
            return res;
        }

        /// <summary>
        /// Updates an existing plane in the database.
        /// </summary>
        /// <param name="id">The unique identifier of the plane to be updated.</param>
        /// <param name="planeRequest">The updated details of the plane.</param>
        /// <returns cref="MainResponse{PlaneResponse}">
        /// A MainResponse containing information about the updated plane if successful, 
        /// otherwise, a MainResponse with the appropriate error message.
        /// </returns>
        public async Task<MainResponse<PlaneResponse>> UpdatePlane(int id, PlaneRequest planeRequest)
        {
            MainResponse<PlaneResponse> res = new();
            var message = "Informations provided corresponds to an existing plane";
            try
            {
                var query = new PlaneExistenceQuery(planeRequest);
                var IsPlaneExists = await _mediator.Send(query);
                if (!IsPlaneExists)
                {
                    var request = new UpdatePlaneRequest(id, planeRequest);
                    var updatedPlaneResponse = await _mediator.Send(request);
                    if (updatedPlaneResponse != null)
                    {
                        res.Data = updatedPlaneResponse;
                        res.Success = true;
                        message = "Plane Updated Successfully";
                    }
                    else
                    {
                        message = "Plane Not Found";
                    }
                }  
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{repo} An error Occured: {ErrorMessage}", typeof(PlaneService), ex.Message);
                message = ex.Message;
            }
            res.Message = message;
            return res;
        }

        /// <summary>
        /// Deletes a plane from the database.
        /// </summary>
        /// <param name="id">The unique identifier of the plane to be deleted.</param>
        /// <returns cref="MainResponse{bool}">
        /// A MainResponse indicating the success of the delete operation, 
        /// otherwise, a MainResponse with the appropriate error message.
        /// </returns>
        public async Task<MainResponse<bool>> DeletePlane(int id)
        {
            MainResponse<bool> res = new();
            var message = "Plane Not Found";
            try
            {
                var request = new DeletePlaneRequest(id);
                if( await _mediator.Send(request))
                {
                    message = "Plane Deleted Successfully";
                    res.Success = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{repo} An error Occured: {ErrorMessage}", typeof(PlaneService), ex.Message);
                message = ex.Message;
            }
            res.Message = message;
            return res;
        }
    }
}
