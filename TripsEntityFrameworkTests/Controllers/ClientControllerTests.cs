using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using TripsEntityFramework.Controllers;
using TripsEntityFramework.Interfaces;
using TripsEntityFramework;

namespace TripsEntityFrameworkTests.Controllers
{
    public class ClientControllerTests
    {
        private readonly Mock<IClientService> _clientServiceMock;
        private readonly ClientController _clientController;

        public ClientControllerTests()
        {
            _clientServiceMock = new Mock<IClientService>();
            _clientController = new ClientController(_clientServiceMock.Object);
        }

        [Fact]
        public async Task DeleteClient_WhenClientDoesNotExist_ShouldReturnBadRequest()
        {
            // Arrange
            int id = 1;
            CancellationToken cancellationToken = new CancellationToken();
            _clientServiceMock.Setup(x => x.DeleteClientAsync(id, cancellationToken)).ReturnsAsync(false);

            // Act
            var result = await _clientController.DeleteClient(id, cancellationToken);

            // Assert
            var requestResult = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(requestResult.Value);
            Assert.Equal("Cannot delete client. Either the client does not exist or the client is assigned to one or more trips.", apiResponse.Message);
            Assert.False(apiResponse.Success);
            Assert.Null(apiResponse.Data);
        }

        [Fact]
        public async Task DeleteClient_ClientDeletedSuccessfully_ReturnsOk()
        {
            // Arrange
            int id = 1;
            CancellationToken cancellationToken = new CancellationToken();
            _clientServiceMock.Setup(x => x.DeleteClientAsync(id, cancellationToken)).ReturnsAsync(true);

            // Act
            var result = await _clientController.DeleteClient(id, cancellationToken);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(okResult.Value);
            Assert.Equal("Client deleted successfully.", apiResponse.Message);
            Assert.True(apiResponse.Success);
            Assert.Null(apiResponse.Data);
        }
    }
}
