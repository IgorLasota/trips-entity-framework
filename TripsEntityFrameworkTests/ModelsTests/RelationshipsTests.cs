using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using TripsEntityFramework.context;
using TripsEntityFramework.Models;

namespace TripsEntityFrameworkTests.ModelsTests
{
    public class RelationshipTests
    {
        [Fact]
        public async Task Can_Add_ClientTrip_Relationship()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "Can_Add_ClientTrip_Relationship")
                .Options;

            await using var context = new DatabaseContext(options);

            var client = new Client
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Telephone = "123456789",
                Pesel = "12345678901"
            };

            var trip = new Trip
            {
                Id = 1,
                Name = "Test Trip",
                Description = "Test Description",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(7),
                MaxPeople = 10
            };

            context.Clients.Add(client);
            context.Trips.Add(trip);
            await context.SaveChangesAsync();

            var clientTrip = new ClientTrip
            {
                IdClient = client.Id,
                IdTrip = trip.Id,
                RegisteredAt = DateTime.UtcNow
            };

            // Act
            context.ClientTrips.Add(clientTrip);
            await context.SaveChangesAsync();

            // Assert
            var savedClientTrip = await context.ClientTrips
                .Include(ct => ct.Client)
                .Include(ct => ct.Trip)
                .FirstOrDefaultAsync(ct => ct.IdClient == client.Id && ct.IdTrip == trip.Id);

            Assert.NotNull(savedClientTrip);
            Assert.Equal(client.Id, savedClientTrip.Client.Id);
            Assert.Equal(trip.Id, savedClientTrip.Trip.Id);
        }

        [Fact]
        public async Task Deleting_Client_Removes_ClientTrip_Relationship()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "Deleting_Client_Removes_ClientTrip_Relationship")
                .Options;

            await using var context = new DatabaseContext(options);

            var client = new Client
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Telephone = "123456789",
                Pesel = "12345678901"
            };

            var trip = new Trip
            {
                Id = 1,
                Name = "Test Trip",
                Description = "Test Description",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(7),
                MaxPeople = 10
            };

            context.Clients.Add(client);
            context.Trips.Add(trip);
            await context.SaveChangesAsync();

            var clientTrip = new ClientTrip
            {
                IdClient = client.Id,
                IdTrip = trip.Id,
                RegisteredAt = DateTime.UtcNow
            };

            context.ClientTrips.Add(clientTrip);
            await context.SaveChangesAsync();

            // Act
            context.Clients.Remove(client);
            await context.SaveChangesAsync();

            // Assert
            var savedClientTrip = await context.ClientTrips
                .FirstOrDefaultAsync(ct => ct.IdClient == client.Id && ct.IdTrip == trip.Id);

            Assert.Null(savedClientTrip);
        }

        [Fact]
        public async Task Can_Add_CountryTrip_Relationship()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "Can_Add_CountryTrip_Relationship")
                .Options;

            await using var context = new DatabaseContext(options);

            var country = new Country
            {
                Id = 1,
                Name = "Test Country"
            };

            var trip = new Trip
            {
                Id = 1,
                Name = "Test Trip",
                Description = "Test Description",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(7),
                MaxPeople = 10
            };

            context.Countries.Add(country);
            context.Trips.Add(trip);
            await context.SaveChangesAsync();

            var countryTrip = new CountryTrip
            {
                IdCounty = country.Id,
                IdTrip = trip.Id
            };

            // Act
            context.CountryTrips.Add(countryTrip);
            await context.SaveChangesAsync();

            // Assert
            var savedCountryTrip = await context.CountryTrips
                .Include(ct => ct.Country)
                .Include(ct => ct.Trip)
                .FirstOrDefaultAsync(ct => ct.IdCounty == country.Id && ct.IdTrip == trip.Id);

            Assert.NotNull(savedCountryTrip);
            Assert.Equal(country.Id, savedCountryTrip.Country.Id);
            Assert.Equal(trip.Id, savedCountryTrip.Trip.Id);
        }

        [Fact]
        public async Task Deleting_Country_Removes_CountryTrip_Relationship()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "Deleting_Country_Removes_CountryTrip_Relationship")
                .Options;

            await using var context = new DatabaseContext(options);

            var country = new Country
            {
                Id = 1,
                Name = "Test Country"
            };

            var trip = new Trip
            {
                Id = 1,
                Name = "Test Trip",
                Description = "Test Description",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(7),
                MaxPeople = 10
            };

            context.Countries.Add(country);
            context.Trips.Add(trip);
            await context.SaveChangesAsync();

            var countryTrip = new CountryTrip
            {
                IdCounty = country.Id,
                IdTrip = trip.Id
            };

            context.CountryTrips.Add(countryTrip);
            await context.SaveChangesAsync();

            // Act
            context.Countries.Remove(country);
            await context.SaveChangesAsync();

            // Assert
            var savedCountryTrip = await context.CountryTrips
                .FirstOrDefaultAsync(ct => ct.IdCounty == country.Id && ct.IdTrip == trip.Id);

            Assert.Null(savedCountryTrip);
        }
    }
}
