using DeliveryWebApi.Controllers;
using DeliveryWebApi.DTO;
using DeliveryWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace DeliveryWepApi_Tests.Controllers
{
    [TestFixture(Category = "UnitTests")]
    public class Api2Controller_UnitTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Get_request()
        {
            // Arrange
            var request = new Api2Request { Consignee = "Addr1", Consignor = "Addr2", Cartons = new[] { 1.1, 1.2, 1.3 } };
            var answer = new Api2Result { Amount = 5.5 };
            var offersServiceMock = new Mock<IOffersAppService>();
            offersServiceMock.Setup(p =>
                    p.Calculate(
                        It.Is<string>(x => x == request.Consignee),
                        It.Is<string>(x => x == request.Consignor),
                        It.Is<double[]>(x => x == request.Cartons)))
                .Returns(answer.Amount);

            // Act
            var controller = new Api2Controller(offersServiceMock.Object);
            var result = controller.Get(request);

            // Assert
            offersServiceMock.Verify(p =>
                    p.Calculate(
                        It.Is<string>(x => x == request.Consignee),
                        It.Is<string>(x => x == request.Consignor),
                        It.Is<double[]>(x => x == request.Cartons)),
                Times.Once);

            Assert.IsNotNull(result?.Value);
            Assert.AreEqual(answer.Amount, result.Value.Amount);
        }

        [Test]
        public void Get_request_NotFound()
        {
            // Arrange
            var request = new Api2Request { Consignee = "Addr1", Consignor = "Addr2", Cartons = new[] { 1.1, 1.2, 1.3 } };
            var offersServiceMock = new Mock<IOffersAppService>();
            offersServiceMock.Setup(p =>
                    p.Calculate(
                        It.Is<string>(x => x == request.Consignee),
                        It.Is<string>(x => x == request.Consignor),
                        It.Is<double[]>(x => x == request.Cartons)))
                .Returns(double.NaN);

            // Act
            var controller = new Api2Controller(offersServiceMock.Object);
            var result = controller.Get(request);

            // Assert
            offersServiceMock.Verify(p =>
                    p.Calculate(
                        It.Is<string>(x => x == request.Consignee),
                        It.Is<string>(x => x == request.Consignor),
                        It.Is<double[]>(x => x == request.Cartons)),
                Times.Once);

            Assert.IsNotNull(result?.Result);
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }
    }
}