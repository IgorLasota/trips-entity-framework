namespace TripsEntityFramework.Interfaces;

public interface IClientService
{
    Task<bool> DeleteClientAsync(int id, CancellationToken cancellationToken);
}