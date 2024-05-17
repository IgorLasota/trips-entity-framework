using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using TripsEntityFramework.Controllers;
using TripsEntityFramework.Interfaces;
using TripsEntityFramework;
using TripsEntityFramework.Requests;

namespace TripsEntityFrameworkTests.Controllers
{
    public class TripsControllerTests
    {
        private readonly Mock<ITripService> _tripServiceMock;
        private readonly TripsController _tripsController;

        public TripsControllerTests()
        {
            _tripServiceMock = new Mock<ITripService>();
            _tripsController = new TripsController(_tripServiceMock.Object);
        }

        [Fact]
        public async Task GetTrips_ReturnsTripsSortedByStartDateDescending()
        {
            // Arrange
            var trips = new List<TripDto>
            {
                new TripDto { Id = 1, Name = "Trip 1", Description = "Description 1", StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(7), MaxPeople = 10 },
                new TripDto { Id = 2, Name = "Trip 2", Description = "Description 2", StartDate = DateTime.UtcNow.AddDays(1), EndDate = DateTime.UtcNow.AddDays(8), MaxPeople = 20 }
            };

            _tripServiceMock.Setup(service => service.GetTripsSortedByStartDateDescendingAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(trips);

            // Act
            var result = await _tripsController.GetTrips(CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<List<TripDto>>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(trips, apiResponse.Data);
        }

        [Fact]
        public async Task AssignClientToTrip_Failure_ReturnsBadRequest()
        {
            // Arrange
            int tripId = 1;
            var requestDto = new AssignClientToTripRequestDto { Pesel = "12345678901" };
            string errorMessage = "Client is already assigned to this trip.";
            _tripServiceMock.Setup(service => service.AssignClientToTripAsync(tripId, requestDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync((false, errorMessage, null));

            // Act
            var result = await _tripsController.AssignClientToTrip(tripId, requestDto, CancellationToken.None);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(badRequestResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal(errorMessage, apiResponse.Message);
            Assert.Null(apiResponse.Data);
        }

        [Fact]
        public async Task AssignClientToTrip_Success_ReturnsOk()
        {
            // Arrange
            int tripId = 1;
            var requestDto = new AssignClientToTripRequestDto { Pesel = "12345678901" };
            var responseDto = new AssignClientToTripResponseDto { ClientId = 1, TripId = 1, ClientName = "John Doe", TripName = "Test Trip", RegisteredAt = DateTime.UtcNow };
            _tripServiceMock.Setup(service => service.AssignClientToTripAsync(tripId, requestDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync((true, null, responseDto));

            // Act
            var result = await _tripsController.AssignClientToTrip(tripId, requestDto, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<AssignClientToTripResponseDto>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(responseDto, apiResponse.Data);
            Assert.Null(apiResponse.Message);
        }
    }
}
