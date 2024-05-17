using System.ComponentModel.DataAnnotations;
using TripsEntityFramework.Models;

namespace TripsEntityFrameworkTests.ModelsTests
{
    public class ModelTests
    {
        [Fact]
        public void Client_Validation_RequiredFields()
        {
            // Arrange
            var client = new Client();

            // Act
            var validationContext = new ValidationContext(client, null, null);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(client, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, v => v.MemberNames.Contains("FirstName"));
            Assert.Contains(validationResults, v => v.MemberNames.Contains("LastName"));
            Assert.Contains(validationResults, v => v.MemberNames.Contains("Email"));
            Assert.Contains(validationResults, v => v.MemberNames.Contains("Telephone"));
            Assert.Contains(validationResults, v => v.MemberNames.Contains("Pesel"));
        }

        [Fact]
        public void Country_Validation_RequiredFields()
        {
            // Arrange
            var country = new Country();

            // Act
            var validationContext = new ValidationContext(country, null, null);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(country, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, v => v.MemberNames.Contains("Name"));
        }

        [Fact]
        public void ClientTrip_Validation_NoRequiredFields()
        {
            // Arrange
            var clientTrip = new ClientTrip
            {
                IdClient = 1,
                IdTrip = 1,
                RegisteredAt = DateTime.UtcNow
            };

            // Act
            var validationContext = new ValidationContext(clientTrip, null, null);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(clientTrip, validationContext, validationResults, true);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void CountryTrip_Validation_NoRequiredFields()
        {
            // Arrange
            var countryTrip = new CountryTrip
            {
                IdCounty = 1,
                IdTrip = 1
            };

            // Act
            var validationContext = new ValidationContext(countryTrip, null, null);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(countryTrip, validationContext, validationResults, true);

            // Assert
            Assert.True(isValid);
        }
    }
}
