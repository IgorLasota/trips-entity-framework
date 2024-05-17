using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using TripsEntityFramework.context;
using TripsEntityFramework.Models;
using TripsEntityFramework.Services;

namespace TripsEntityFrameworkTests
{
    public class ClientServiceTests
    {
        [Fact]
        public async Task DeleteClientAsync_ClientNotFound_ReturnsFalse()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "DeleteClientAsync_ClientNotFound_ReturnsFalse")
                .Options;

            await using var context = new DatabaseContext(options);
            var service = new ClientService(context);

            // Act
            var result = await service.DeleteClientAsync(1, CancellationToken.None);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteClientAsync_ClientWithTrips_ReturnsFalse()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "DeleteClientAsync_ClientWithTrips_ReturnsFalse")
                .Options;

            await using var context = new DatabaseContext(options);
            var client = new Client
            {
                Id = 1,
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                Pesel = "12345678901",
                Telephone = "123456789"
            };
            context.Clients.Add(client);
            context.ClientTrips.Add(new ClientTrip { IdClient = 1, IdTrip = 1 });
            await context.SaveChangesAsync();

            var service = new ClientService(context);

            // Act
            var result = await service.DeleteClientAsync(1, CancellationToken.None);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteClientAsync_ClientWithoutTrips_ReturnsTrue()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "DeleteClientAsync_ClientWithoutTrips_ReturnsTrue")
                .Options;

            await using var context = new DatabaseContext(options);
            var client = new Client
            {
                Id = 1,
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                Pesel = "12345678901",
                Telephone = "123456789"
            };
            context.Clients.Add(client);
            await context.SaveChangesAsync();

            var service = new ClientService(context);

            // Act
            var result = await service.DeleteClientAsync(1, CancellationToken.None);

            // Assert
            Assert.True(result);
            Assert.Null(await context.Clients.FindAsync(1));
        }
        
        [Fact]
        public async Task DeleteClientAsync_ClientWithTrips_DoesNotRemoveClient()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "DeleteClientAsync_ClientWithTrips_DoesNotRemoveClient")
                .Options;

            await using var context = new DatabaseContext(options);
            var client = new Client
            {
                Id = 1,
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                Pesel = "12345678901",
                Telephone = "123456789"
            };
            context.Clients.Add(client);
            context.ClientTrips.Add(new ClientTrip { IdClient = 1, IdTrip = 1 });
            await context.SaveChangesAsync();

            var service = new ClientService(context);

            // Act
            var result = await service.DeleteClientAsync(1, CancellationToken.None);

            // Assert
            Assert.False(result);
            Assert.NotNull(await context.Clients.FindAsync(1));
        }
        
        [Fact]
        public async Task DeleteClientAsync_OperationCancelled_ThrowsOperationCanceledException()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "DeleteClientAsync_OperationCancelled_ThrowsOperationCanceledException")
                .Options;

            await using var context = new DatabaseContext(options);
            var client = new Client
            {
                Id = 1,
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                Pesel = "12345678901",
                Telephone = "123456789"
            };
            context.Clients.Add(client);
            await context.SaveChangesAsync();

            var service = new ClientService(context);
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // Act & Assert
            await Assert.ThrowsAsync<OperationCanceledException>(() => service.DeleteClientAsync(1, cancellationTokenSource.Token));
        }
    }
}
