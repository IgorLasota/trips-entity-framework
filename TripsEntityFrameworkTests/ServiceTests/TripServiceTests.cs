using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TripsEntityFramework.context;
using TripsEntityFramework.Models;
using TripsEntityFramework.Requests;
using TripsEntityFramework.Services;
using Xunit;

namespace TripsEntityFrameworkTests.ServiceTests
{
    public class TripServiceTests
    {
        [Fact]
        public async Task GetTripsSortedByStartDateDescendingAsync_ReturnsSortedTrips()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "GetTripsSortedByStartDateDescendingAsync_ReturnsSortedTrips")
                .Options;

            await using var context = new DatabaseContext(options);
            var trips = new List<Trip>
            {
                new Trip { Id = 1, Name = "Trip 1", StartDate = new DateTime(2023, 5, 1), Description = "Description 1" },
                new Trip { Id = 2, Name = "Trip 2", StartDate = new DateTime(2023, 6, 1), Description = "Description 2" },
                new Trip { Id = 3, Name = "Trip 3", StartDate = new DateTime(2023, 4, 1), Description = "Description 3" }
            };
            context.Trips.AddRange(trips);
            await context.SaveChangesAsync();

            var service = new TripService(context);

            // Act
            var result = await service.GetTripsSortedByStartDateDescendingAsync(CancellationToken.None);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal(2, result[0].Id);
            Assert.Equal(1, result[1].Id);
            Assert.Equal(3, result[2].Id);
        }

        [Fact]
        public async Task GetTripsSortedByStartDateDescendingAsync_ReturnsEmptyList_WhenNoTrips()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "GetTripsSortedByStartDateDescendingAsync_ReturnsEmptyList_WhenNoTrips")
                .Options;

            await using var context = new DatabaseContext(options);
            var service = new TripService(context);

            // Act
            var result = await service.GetTripsSortedByStartDateDescendingAsync(CancellationToken.None);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetTripsSortedByStartDateDescendingAsync_OperationCancelled_ThrowsOperationCanceledException()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "GetTripsSortedByStartDateDescendingAsync_OperationCancelled_ThrowsOperationCanceledException")
                .Options;

            await using var context = new DatabaseContext(options);
            var trips = new List<Trip>
            {
                new Trip { Id = 1, Name = "Trip 1", StartDate = new DateTime(2023, 5, 1), Description = "Description 1" },
                new Trip { Id = 2, Name = "Trip 2", StartDate = new DateTime(2023, 6, 1), Description = "Description 2" },
                new Trip { Id = 3, Name = "Trip 3", StartDate = new DateTime(2023, 4, 1), Description = "Description 3" }
            };
            context.Trips.AddRange(trips);
            await context.SaveChangesAsync();

            var service = new TripService(context);
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // Act & Assert
            await Assert.ThrowsAsync<OperationCanceledException>(() => service.GetTripsSortedByStartDateDescendingAsync(cancellationTokenSource.Token));
        }
        
        [Fact]
        public async Task AssignClientToTripAsync_TripNotFound_ReturnsError()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "AssignClientToTripAsync_TripNotFound_ReturnsError")
                .Options;

            await using var context = new DatabaseContext(options);
            var service = new TripService(context);
            var requestDto = new AssignClientToTripRequestDto 
            { 
                Pesel = "12345678901",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Telephone = "123456789",
                IdTrip = 1,
                TripName = "Test Trip",
                PaymentDate = null
            };

            // Act
            var result = await service.AssignClientToTripAsync(1, requestDto, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Trip not found.", result.ErrorMessage);
        }
        
        [Fact]
        public async Task AssignClientToTripAsync_ClientAlreadyAssigned_ReturnsError()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "AssignClientToTripAsync_ClientAlreadyAssigned_ReturnsError")
                .Options;

            await using var context = new DatabaseContext(options);
            var trip = new Trip { Id = 1, Name = "Test Trip", Description = "Test Description", StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(7), MaxPeople = 10 };
            var client = new Client
            {
                Id = 1,
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                Pesel = "12345678901",
                Telephone = "123456789"
            };
            var clientTrip = new ClientTrip { IdClient = 1, IdTrip = 1, RegisteredAt = DateTime.UtcNow };

            context.Trips.Add(trip);
            context.Clients.Add(client);
            context.ClientTrips.Add(clientTrip);
            await context.SaveChangesAsync();

            var service = new TripService(context);
            var requestDto = new AssignClientToTripRequestDto 
            { 
                Pesel = "12345678901",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Telephone = "123456789",
                IdTrip = 1,
                TripName = "Test Trip",
                PaymentDate = null
            };

            // Act
            var result = await service.AssignClientToTripAsync(1, requestDto, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Client is already assigned to this trip.", result.ErrorMessage);
        }

        [Fact]
        public async Task AssignClientToTripAsync_ClientExists_AssignsClientToTrip()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "AssignClientToTripAsync_ClientExists_AssignsClientToTrip")
                .Options;

            await using var context = new DatabaseContext(options);
            var trip = new Trip { Id = 1, Name = "Test Trip", Description = "Test Description", StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(7), MaxPeople = 10 };
            var client = new Client
            {
                Id = 1,
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                Pesel = "12345678901",
                Telephone = "123456789"
            };

            context.Trips.Add(trip);
            context.Clients.Add(client);
            await context.SaveChangesAsync();

            var service = new TripService(context);
            var requestDto = new AssignClientToTripRequestDto
            {
                Pesel = "12345678901",
                FirstName = "John",
                LastName = "Doe",
                Email = "test@example.com",
                Telephone = "123456789",
                IdTrip = 1,
                TripName = "Test Trip",
                PaymentDate = null
            };

            // Act
            var result = await service.AssignClientToTripAsync(1, requestDto, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Response);
            Assert.Equal(1, result.Response.TripId);
            Assert.Equal(1, result.Response.ClientId);
        }

        [Fact]
        public async Task AssignClientToTripAsync_ClientDoesNotExist_AssignsClientToTrip()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "AssignClientToTripAsync_ClientDoesNotExist_AssignsClientToTrip")
                .Options;

            await using var context = new DatabaseContext(options);
            var trip = new Trip { Id = 1, Name = "Test Trip", Description = "Test Description", StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(7), MaxPeople = 10 };

            context.Trips.Add(trip);
            await context.SaveChangesAsync();

            var service = new TripService(context);
            var requestDto = new AssignClientToTripRequestDto
            {
                Pesel = "12345678901",
                FirstName = "John",
                LastName = "Doe",
                Email = "test@example.com",
                Telephone = "123456789",
                IdTrip = 1,
                TripName = "Test Trip",
                PaymentDate = null
            };

            // Act
            var result = await service.AssignClientToTripAsync(1, requestDto, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Response);
            Assert.Equal(1, result.Response.TripId);
            var client = await context.Clients.FirstOrDefaultAsync(c => c.Pesel == "12345678901");
            Assert.NotNull(client);
        }
    }
}
