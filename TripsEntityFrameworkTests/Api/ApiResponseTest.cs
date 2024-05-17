using System.Text.Json;
using TripsEntityFramework;
using Xunit;

namespace TripsEntityFrameworkTests.Api
{
    public class ApiResponseTests
    {
        [Fact]
        public void ApiResponse_Success_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var data = new { Name = "Test", Value = 123 };
            var response = new ApiResponse<object>(true, null, data);

            // Act & Assert
            Assert.True(response.Success);
            Assert.Null(response.Message);
            Assert.Equal(data, response.Data);
        }

        [Fact]
        public void ApiResponse_Error_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var message = "An error occurred";
            var response = new ApiResponse<object>(false, message, null);

            // Act & Assert
            Assert.False(response.Success);
            Assert.Equal(message, response.Message);
            Assert.Null(response.Data);
        }

        [Fact]
        public void ApiResponse_ErrorWithData_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var message = "An error occurred";
            var data = new { Name = "Test", Value = 123 };
            var response = new ApiResponse<object>(false, message, data);

            // Act & Assert
            Assert.False(response.Success);
            Assert.Equal(message, response.Message);
            Assert.Equal(data, response.Data);
        }

        [Fact]
        public void ApiResponse_ToString_ShouldReturnValidJson()
        {
            // Arrange
            var data = new { Name = "Test", Value = 123 };
            var response = new ApiResponse<object>(true, null, data);
            var expectedJson = JsonSerializer.Serialize(new
            {
                Success = true,
                Message = (string)null,
                Data = data
            });

            // Act
            var json = response.ToString();

            // Assert
            Assert.Equal(expectedJson, json);
        }

        [Fact]
        public void ApiResponse_ErrorToString_ShouldReturnValidJson()
        {
            // Arrange
            var message = "An error occurred";
            var response = new ApiResponse<object>(false, message, null);
            var expectedJson = JsonSerializer.Serialize(new
            {
                Success = false,
                Message = message,
                Data = (object)null
            });

            // Act
            var json = response.ToString();

            // Assert
            Assert.Equal(expectedJson, json);
        }
    }
}
