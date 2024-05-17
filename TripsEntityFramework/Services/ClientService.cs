    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using TripsEntityFramework.context;
    using TripsEntityFramework.Interfaces;
    using TripsEntityFramework.Models;

    namespace TripsEntityFramework.Services
    {
        public class ClientService : IClientService
        {
            private readonly DatabaseContext _dbContext;

            public ClientService(DatabaseContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<bool> DeleteClientAsync(int id, CancellationToken cancellationToken)
            {
                var client = await _dbContext.Clients.FindAsync(new object[] { id }, cancellationToken);
                if (client == null)
                {
                    return false;
                }

                var assignedTrips = await _dbContext.ClientTrips
                    .Where(ct => ct.IdClient == id)
                    .ToListAsync(cancellationToken);

                if (assignedTrips.Any())
                {
                    return false;
                }

                _dbContext.Clients.Remove(client);
                await _dbContext.SaveChangesAsync(cancellationToken);
                
                return true;
            }
        }
    }