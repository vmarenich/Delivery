using System.Collections.Generic;
using System.Linq;
using DeliveryWebApi.Controllers;
using DeliveryWebApi.Domain;
using DeliveryWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace DeliveryWepApi_Tests.Controllers
{
    [TestFixture(Category = "UnitTests")]
    public class FillController_UnitTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Get()
        {
            // Arrange
            var offer1 = new Offer { Id = 1, SupplierId = 2, SourceAddress = "Addr1", DestAddress = "Addr2", Price = 1.1 };
            var offer2 = new Offer { Id = 2, SupplierId = 3, SourceAddress = "Addr1", DestAddress = "Addr2", Price = 1.2 };
            var offers = new List<Offer> { offer1, offer2 };
            var offersServiceMock = new Mock<IOffersAppService>();
            offersServiceMock.Setup(p => p.GetAll()).Returns(offers);

            // Act
            var controller = new FillController(offersServiceMock.Object);
            var result = controller.Get();

            // Assert
            offersServiceMock.Verify(p => p.GetAll(), Times.Once);
            Assert.IsNotNull(result?.Value);
            Assert.AreEqual(2, result.Value.Count);
            Assert.AreSame(offer1, result.Value[0]);
            Assert.AreSame(offer2, result.Value[1]);
        }

        [TestCase(1)]
        [TestCase(2)]
        public void GetOffer_id(int id)
        {
            // Arrange
            var offer1 = new Offer { Id = 1, SupplierId = 2, SourceAddress = "Addr1", DestAddress = "Addr2", Price = 1.1 };
            var offer2 = new Offer { Id = 2, SupplierId = 3, SourceAddress = "Addr1", DestAddress = "Addr2", Price = 1.2 };
            var offers = new List<Offer> { offer1, offer2 };
            var found = offers.FirstOrDefault(p => p.Id == id);
            var offersServiceMock = new Mock<IOffersAppService>();
            offersServiceMock.Setup(p => p.Get(It.Is<int>(x => x == id))).Returns(found);

            // Act
            var controller = new FillController(offersServiceMock.Object);
            var result = controller.GetOffer(id);

            // Assert
            Assert.IsNotNull(found, $"Invalid TestCase({id})");
            offersServiceMock.Verify(p => p.Get(It.Is<int>(x => x == id)), Times.Once);
            Assert.IsNotNull(result?.Value);
            Assert.AreSame(found, result.Value);
        }

        [Test]
        public void GetOffer_id_NotFound()
        {
            // Arrange
            var id = 555;
            var offersServiceMock = new Mock<IOffersAppService>();
            offersServiceMock.Setup(p => p.Get(It.Is<int>(x => x == id))).Returns(default(Offer));

            // Act
            var controller = new FillController(offersServiceMock.Object);
            var result = controller.GetOffer(id);

            // Assert
            offersServiceMock.Verify(p => p.Get(It.Is<int>(x => x == id)), Times.Once);
            Assert.IsNotNull(result?.Result);
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public void Create_offer()
        {
            // Arrange
            var offer = new Offer { SupplierId = 2, SourceAddress = "Addr1", DestAddress = "Addr2", Price = 1.1 };
            var offersServiceMock = new Mock<IOffersAppService>();
            offersServiceMock.Setup(p => p.AddOrUpdate(It.Is<Offer>(x => ReferenceEquals(x, offer)))).Returns(offer);

            // Act
            var controller = new FillController(offersServiceMock.Object);
            var result = controller.Create(offer);

            // Assert
            offersServiceMock.Verify(p => p.AddOrUpdate(It.Is<Offer>(x => ReferenceEquals(x, offer))), Times.Once);
            Assert.IsNotNull(result?.Result);
            Assert.IsInstanceOf<CreatedAtRouteResult>(result.Result);
            var createdAtRouteResult = (CreatedAtRouteResult)result.Result;
            Assert.AreSame(offer, createdAtRouteResult.Value);
        }
    }
}