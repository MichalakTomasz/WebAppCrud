using Domain.Models;
using WebAppCrud.Validators;

namespace TestProject.UnitTests
{
    public class ValidatorTests
    {
        [Fact]
        public void ProductValidatorValidTestPassed()
        {
            // Arrange

            InputProductValidator validator = new();

            InputProduct inputProduct = new()
            {
                Code = "Tv",
                Name = "Telewizor",
                Description = "Telewizor Xiaomi",
                UrlPicture = "https://sklep.tv.pl",
                Price = 1600
            };

            // Act

            var validationResult = validator.Validate(inputProduct);

            // Assert
            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public void ProductValidatorNotValidTestPassed()
        {
            // Arrange

            InputProductValidator validator = new();

            InputProduct inputProduct = new()
            {
                Code = "Telewizor Xiaomi w promocji",
                Name = "",
                Description = "Telewizor Xiaomi",
                UrlPicture = "htt//sklep.tv.pl",
                Price = -4
            };

            // Act

            var validationResult = validator.Validate(inputProduct);

            // Assert
            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public void ProductValidatorCodeNotValidTestPassed()
        {
            // Arrange

            InputProductValidator validator = new();

            InputProduct inputProduct = new()
            {
                Code = "Telewizor Xiaomi Xv1600",
                Name = "Telewizor",
                Description = "Telewizor Xiaomi",
                UrlPicture = "https://sklep.tv.pl",
                Price = 1600
            };

            // Act

            var validationResult = validator.Validate(inputProduct);

            // Assert
            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public void ProductValidatorNameNotValidTestPassed()
        {
            // Arrange

            InputProductValidator validator = new();

            InputProduct inputProduct = new()
            {
                Code = "Tv",
                Name = "",
                Description = "Telewizor Xiaomi",
                UrlPicture = "https://sklep.tv.pl",
                Price = 1600
            };

            // Act

            var validationResult = validator.Validate(inputProduct);

            // Assert
            Assert.False(validationResult.IsValid);
        }
    }
}
