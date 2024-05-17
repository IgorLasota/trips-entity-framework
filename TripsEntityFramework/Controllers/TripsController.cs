using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TripsEntityFramework.Interfaces;
using TripsEntityFramework.Requests;

namespace TripsEntityFramework.Controllers
{
    [Route("api/trips")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly ITripService _tripService;

        public TripsController(ITripService tripService)
        {
            _tripService = tripService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrips(CancellationToken cancellationToken)
        {
            var trips = await _tripService.GetTripsSortedByStartDateDescendingAsync(cancellationToken);
            return Ok(new ApiResponse<List<TripDto>>(true, null, trips));
        }

        [HttpPost("{tripId}/clients")]
        public async Task<IActionResult> AssignClientToTrip(int tripId, [FromBody] AssignClientToTripRequestDto requestDto, CancellationToken cancellationToken)
        {
            var result = await _tripService.AssignClientToTripAsync(tripId, requestDto, cancellationToken);
            if (!result.IsSuccess)
            {
                return BadRequest(new ApiResponse<string>(false, result.ErrorMessage, null));
            }
            return Ok(new ApiResponse<AssignClientToTripResponseDto>(true, null, result.Response));
        }
    }
}