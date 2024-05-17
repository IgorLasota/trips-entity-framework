using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TripsEntityFramework.Models;
using TripsEntityFramework.Requests;

namespace TripsEntityFramework.Interfaces;

public interface ITripService
{
    Task<List<TripDto>> GetTripsSortedByStartDateDescendingAsync(CancellationToken cancellationToken);

    Task<(bool IsSuccess, string ErrorMessage, AssignClientToTripResponseDto Response)> AssignClientToTripAsync(int tripId, AssignClientToTripRequestDto requestDto, CancellationToken cancellationToken);
    
}
