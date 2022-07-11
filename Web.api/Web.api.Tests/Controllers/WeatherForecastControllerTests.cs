using Microsoft.Extensions.Logging;
using Moq;
using Web.api.Controllers;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System;

namespace Web.api.Tests.Controllers
{
    public class WeatherForecastControllerTests
    {
        private readonly Mock<ILogger<WeatherForecastController>> _loggerMock;

        public WeatherForecastControllerTests()
        {
            _loggerMock = new Mock<ILogger<WeatherForecastController>>();
        }

        private WeatherForecastController CreateSubject() => new(_loggerMock.Object);

        [Fact]
        public void Get_Should_Return_Weather_For_5_Days()
        {
            //arrange
            var subject = CreateSubject();

            //act
            var result = subject.Get();

            //assert
            result.Should().BeOfType<WeatherForecast[]>();
            var weatherForecasts = result.As<WeatherForecast[]>();
            weatherForecasts.Should().NotBeNull();
            weatherForecasts.Should().HaveCount(c => c == 5)
                .And.OnlyHaveUniqueItems();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void Get_Should_Return_Weather_For_Max_Provided_Days(int maxNumberOfDays)
        {
            //arrange
            var subject = CreateSubject();

            //act
            var result = subject.Get(maxNumberOfDays);

            //assert
            result.Should().BeOfType<WeatherForecast[]>();
            var weatherForecasts = result.As<WeatherForecast[]>();
            weatherForecasts.Should().NotBeNull();
            weatherForecasts.Should().HaveCount(c => c == maxNumberOfDays)
                .And.OnlyHaveUniqueItems();
        }

        [Theory]
        [InlineData(-10)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(4)]
        [InlineData(10)]
        [InlineData(14)]
        public void Get_Should_Return_Weather_Correct_Result_For_Provided_Days(int maxNumberOfDays)
        {
            //arrange
            var subject = CreateSubject();

            //act
            var result = subject.Get(maxNumberOfDays);

            //assert
            result.Should().BeOfType<WeatherForecast[]>();
            var weatherForecasts = result.As<WeatherForecast[]>();
            weatherForecasts.Should().NotBeNull();

            if (maxNumberOfDays > 0 && maxNumberOfDays < 5)
            {
                weatherForecasts.Should().HaveCount(c => c == maxNumberOfDays)
                    .And.OnlyHaveUniqueItems();
            }
            else if (maxNumberOfDays > 5)
            {
                weatherForecasts.Should().HaveCount(c => c == 5)
                    .And.OnlyHaveUniqueItems();
            }
            else
            {
                weatherForecasts.Should().HaveCount(c => c == 0);
            }
        }

    }
}
