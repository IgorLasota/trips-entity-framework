using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TripsEntityFramework.context;
using TripsEntityFramework.Interfaces;
using TripsEntityFramework.Models;
using TripsEntityFramework.Requests;

namespace TripsEntityFramework.Services
{
    public class TripService : ITripService
    {
        private readonly DatabaseContext _context;

        public TripService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<TripDto>> GetTripsSortedByStartDateDescendingAsync(CancellationToken cancellationToken)
        {
            return await _context.Trips
                .OrderByDescending(t => t.StartDate)
                .Select(t => new TripDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate,
                    MaxPeople = t.MaxPeople
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<(bool IsSuccess, string ErrorMessage, AssignClientToTripResponseDto Response)> AssignClientToTripAsync(int tripId, AssignClientToTripRequestDto requestDto, CancellationToken cancellationToken)
        {
            var trip = await _context.Trips.FirstOrDefaultAsync(t => t.Id == tripId, cancellationToken);
            if (trip == null)
            {
                return (false, "Trip not found.", null);
            }

            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Pesel == requestDto.Pesel, cancellationToken);
            if (client == null)
            {
                client = new Client
                {
                    FirstName = requestDto.FirstName,
                    LastName = requestDto.LastName,
                    Email = requestDto.Email,
                    Telephone = requestDto.Telephone,
                    Pesel = requestDto.Pesel
                };
                await _context.Clients.AddAsync(client, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            var existingAssignment = await _context.ClientTrips
                .AnyAsync(ct => ct.IdClient == client.Id && ct.IdTrip == tripId, cancellationToken);
            if (existingAssignment)
            {
                return (false, "Client is already assigned to this trip.", null);
            }

            var clientTrip = new ClientTrip
            {
                IdClient = client.Id,
                IdTrip = tripId,
                RegisteredAt = DateTime.UtcNow,
                PaymentDate = requestDto.PaymentDate
            };
            await _context.ClientTrips.AddAsync(clientTrip, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var responseDto = new AssignClientToTripResponseDto
            {
                ClientId = clientTrip.IdClient,
                TripId = clientTrip.IdTrip,
                ClientName = $"{client.FirstName} {client.LastName}",
                TripName = trip.Name,
                RegisteredAt = clientTrip.RegisteredAt,
                PaymentDate = clientTrip.PaymentDate,
                Message = "Client assigned to trip successfully."
            };

            return (true, null, responseDto);
        }
    }
}
