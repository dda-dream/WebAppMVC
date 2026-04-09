using FluentAssertions;

namespace WebAppMVC.Services
{
    public class CalculatorTests
    {
        [Fact]
        public void Add_ShouldReturnCorrectSum()
        {
            // Arrange
            var calculator = new Calculator();

            // Act
            var result = calculator.Add(2, 3);

            // Assert
            Assert.Equal(5, result);
        }

        [Fact]
        public void IsEven_ShouldReturnTrue_ForEvenNumber()
        {
            var calculator = new Calculator();

            var result = calculator.IsEven(4);

            result.Should().BeTrue();
            //Assert.True(result);
        }
    }
}
