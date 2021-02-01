using DeliveryWebApi.Controllers;
using DeliveryWebApi.DTO;
using DeliveryWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace DeliveryWepApi_Tests.Controllers
{
    [TestFixture(Category = "UnitTests")]
    public class Api3Controller_UnitTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Get_request()
        {
            // Arrange
            var request = new Api3Request { Source = "Addr1", Destination = "Addr2", Packages = new[] { 1.1, 1.2, 1.3 } };
            var answer = new Api3Result { Quote = 5.5 };
            var offersServiceMock = new Mock<IOffersAppService>();
            offersServiceMock.Setup(p =>
                    p.Calculate(
                        It.Is<string>(x => x == request.Source),
                        It.Is<string>(x => x == request.Destination),
                        It.Is<double[]>(x => x == request.Packages)))
                .Returns(answer.Quote);

            // Act
            var controller = new Api3Controller(offersServiceMock.Object);
            var result = controller.Get(request);

            // Assert
            offersServiceMock.Verify(p =>
                    p.Calculate(
                        It.Is<string>(x => x == request.Source),
                        It.Is<string>(x => x == request.Destination),
                        It.Is<double[]>(x => x == request.Packages)),
                Times.Once);

            Assert.IsNotNull(result?.Value);
            Assert.AreEqual(answer.Quote, result.Value.Quote);
        }

        [Test]
        public void Get_request_NotFound()
        {
            // Arrange
            var request = new Api3Request { Source = "Addr1", Destination = "Addr2", Packages = new[] { 1.1, 1.2, 1.3 } };
            var offersServiceMock = new Mock<IOffersAppService>();
            offersServiceMock.Setup(p =>
                    p.Calculate(
                        It.Is<string>(x => x == request.Source),
                        It.Is<string>(x => x == request.Destination),
                        It.Is<double[]>(x => x == request.Packages)))
                .Returns(double.NaN);

            // Act
            var controller = new Api3Controller(offersServiceMock.Object);
            var result = controller.Get(request);

            // Assert
            offersServiceMock.Verify(p =>
                    p.Calculate(
                        It.Is<string>(x => x == request.Source),
                        It.Is<string>(x => x == request.Destination),
                        It.Is<double[]>(x => x == request.Packages)),
                Times.Once);

            Assert.IsNotNull(result?.Result);
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }
    }
}