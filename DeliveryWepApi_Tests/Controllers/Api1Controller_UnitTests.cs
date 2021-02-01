using DeliveryWebApi.Controllers;
using DeliveryWebApi.DTO;
using DeliveryWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace DeliveryWepApi_Tests.Controllers
{
    [TestFixture(Category = "UnitTests")]
    public class Api1Controller_UnitTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Get_request()
        {
            // Arrange
            var request = new Api1Request { ContactAddress = "Addr1", WarehouseAddress = "Addr2", CartonDimensions = new[] { 1.1, 1.2, 1.3 } };
            var answer = new Api1Result { Total = 5.5 };
            var offersServiceMock = new Mock<IOffersAppService>();
            offersServiceMock.Setup(p =>
                    p.Calculate(
                        It.Is<string>(x => x == request.ContactAddress),
                        It.Is<string>(x => x == request.WarehouseAddress),
                        It.Is<double[]>(x => x == request.CartonDimensions)))
                .Returns(answer.Total);

            // Act
            var controller = new Api1Controller(offersServiceMock.Object);
            var result = controller.Get(request);

            // Assert
            offersServiceMock.Verify(p =>
                    p.Calculate(
                        It.Is<string>(x => x == request.ContactAddress),
                        It.Is<string>(x => x == request.WarehouseAddress),
                        It.Is<double[]>(x => x == request.CartonDimensions)),
                Times.Once);

            Assert.IsNotNull(result?.Value);
            Assert.AreEqual(answer.Total, result.Value.Total);
        }

        [Test]
        public void Get_request_NotFound()
        {
            // Arrange
            var request = new Api1Request { ContactAddress = "Addr1", WarehouseAddress = "Addr2", CartonDimensions = new[] { 1.1, 1.2, 1.3 } };
            var offersServiceMock = new Mock<IOffersAppService>();
            offersServiceMock.Setup(p =>
                    p.Calculate(
                        It.Is<string>(x => x == request.ContactAddress),
                        It.Is<string>(x => x == request.WarehouseAddress),
                        It.Is<double[]>(x => x == request.CartonDimensions)))
                .Returns(double.NaN);

            // Act
            var controller = new Api1Controller(offersServiceMock.Object);
            var result = controller.Get(request);

            // Assert
            offersServiceMock.Verify(p =>
                    p.Calculate(
                        It.Is<string>(x => x == request.ContactAddress),
                        It.Is<string>(x => x == request.WarehouseAddress),
                        It.Is<double[]>(x => x == request.CartonDimensions)),
                Times.Once);

            Assert.IsNotNull(result?.Result);
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }
    }
}