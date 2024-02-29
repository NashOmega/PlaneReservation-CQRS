using AutoMapper;
using Core.Entities;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;
using Core.Queries.Planes;
using Core.Request;
using Core.Response;
using MediatR;
using Microsoft.Extensions.Logging;

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
        public PlaneService(IUnitOfWork unitOfWork, IMapper mapper, ILoggerFactory factory, IMediator mediator)
            : base(unitOfWork, mapper, factory)
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
                var planeResponse = await _mediator.Send(query);
                if (planeResponse != null)
                {
                    res.Data = planeResponse;
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
            var message = "Plane Created Successfully";
            try
            { 
                PlaneEntity plane = _mapper.Map<PlaneEntity>(planeRequest);
                bool IsPlaneExistsBool = await IsPlaneExists(plane);
                if (!IsPlaneExistsBool)
                {
                    var createdPlane = await _unitOfWork.Planes.CreateAsync(plane);
                    await _unitOfWork.Seats.GeneratePlaneSeats(createdPlane);

                    res.Success = await _unitOfWork.CompleteAsync();
                    res.Data = _mapper.Map<PlaneResponse>(createdPlane);
                }
                else
                {
                    message = "Plane Already Exists";
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
            var message = "Plane Updated Successfully";
            try
            {
                var dbPlane = await _unitOfWork.Planes.FindByIdAsync(id);
                bool IsPlaneExistsBool = await IsPlaneExists(_mapper.Map<PlaneEntity>(planeRequest));
                if (dbPlane != null && !IsPlaneExistsBool)
                {
                    var createdPlane = await _unitOfWork.Planes.UpdateAsync(_mapper.Map(planeRequest, dbPlane));

                    res.Success = await _unitOfWork.CompleteAsync();
                    res.Data = _mapper.Map<PlaneResponse>(createdPlane);              
                }
                else 
                {
                    if (IsPlaneExistsBool) message = "Informations provided corresponds to an existing plane";
                    if(dbPlane == null) message = "Plane Not Found";
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
            var message = "Plane Deleted Successfully";
            try
            {
                var dbPlane = await _unitOfWork.Planes.FindByIdAsync(id);
                if (dbPlane != null)
                {
                    await _unitOfWork.Planes.DeleteAsync(dbPlane);

                   res.Success = await _unitOfWork.CompleteAsync();
                }
                else
                {
                    message = "Plane Not Found";
                    res.Data = false;
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
        /// Checks if a plane with the same attributes as the provided plane exists in the database.
        /// </summary>
        /// <param name="plane">The plane entity to compare against existing planes.</param>
        /// <returns cref="bool">
        /// True if a plane matching the provided criteria exists in the database, otherwise false.
        /// </returns>
        public async Task<bool> IsPlaneExists(PlaneEntity plane)
        {
            var planesMatchingCriteria = await _unitOfWork.Planes.FindByConditionAsync(
                                     p => p.Name == plane.Name && p.Model == plane.Model && p.Serial == plane.Serial);

            return planesMatchingCriteria.Any();
        }
    }
}
