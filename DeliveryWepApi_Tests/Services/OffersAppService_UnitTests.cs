using System;
using System.Collections.Generic;
using System.Linq;
using DeliveryWebApi.DAL;
using DeliveryWebApi.Domain;
using DeliveryWebApi.Services;
using Moq;
using NUnit.Framework;
using Unity;
using Unity.Resolution;

namespace DeliveryWepApi_Tests.Services
{
    [TestFixture(Category = "UnitTests")]
    public class OffersAppService_UnitTests
    {
        protected Mock<IUnityContainer> UnityContainerMock { get; set; }

        [SetUp]
        public void Setup()
        {
            UnityContainerMock = new Mock<IUnityContainer>();
            ServiceUnitTestsHelper.SetupRepository<Supplier>(UnityContainerMock);
        }

        [Test]
        public void GetAll()
        {
            // Arrange
            var offer1 = new Offer { Id = 1, SupplierId = 2, SourceAddress = "Addr1", DestAddress = "Addr2", Price = 1.1 };
            var offer2 = new Offer { Id = 2, SupplierId = 3, SourceAddress = "Addr1", DestAddress = "Addr2", Price = 1.2 };
            var offers = new List<Offer> { offer1, offer2 };
            var offersRepositoryMock = ServiceUnitTestsHelper.SetupRepository<Offer>(UnityContainerMock);
            offersRepositoryMock.SetupGet(p => p.All).Returns(offers.AsQueryable);

            // Act
            var service = new OffersAppService(UnityContainerMock.Object);
            var result = service.GetAll();

            // Assert
            UnityContainerMock.Verify(p =>
                    p.Resolve(
                        It.Is<Type>(x => x == typeof(IRepository<Offer>)),
                        It.Is<string>(x => x == default),
                        It.Is<ResolverOverride[]>(x => x.Length == 0)),
                Times.Once);

            offersRepositoryMock.VerifyGet(p => p.All, Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreSame(offer1, result[0]);
            Assert.AreSame(offer2, result[1]);
        }

        [Test]
        public void Get_id()
        {
            // Arrange
            var offer1 = new Offer { Id = 111, SupplierId = 2, SourceAddress = "Addr1", DestAddress = "Addr2", Price = 1.1 };
            var offer2 = new Offer { Id = 222, SupplierId = 3, SourceAddress = "Addr1", DestAddress = "Addr2", Price = 1.2 };
            var offers = new List<Offer> { offer1, offer2 };
            var offersRepositoryMock = ServiceUnitTestsHelper.SetupRepository<Offer>(UnityContainerMock);
            offersRepositoryMock.SetupGet(p => p.All).Returns(offers.AsQueryable);
            offersRepositoryMock.Setup(p => p.Find(It.Is<int>(x => x == offer1.Id))).Returns(offer1);
            offersRepositoryMock.Setup(p => p.Find(It.Is<int>(x => x == offer2.Id))).Returns(offer2);

            // Act
            var service = new OffersAppService(UnityContainerMock.Object);
            var result = service.Get(offer2.Id);

            // Assert
            UnityContainerMock.Verify(p =>
                    p.Resolve(
                        It.Is<Type>(x => x == typeof(IRepository<Offer>)),
                        It.Is<string>(x => x == default),
                        It.Is<ResolverOverride[]>(x => x.Length == 0)),
                Times.Once);

            offersRepositoryMock.VerifyGet(p => p.All, Times.Never);
            offersRepositoryMock.Verify(p => p.Find(It.Is<int>(x => x == offer1.Id)), Times.Never);
            offersRepositoryMock.Verify(p => p.Find(It.Is<int>(x => x == offer2.Id)), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreSame(offer2, result);
        }

        [Test]
        public void Get_id_NotFound()
        {
            // Arrange
            var id = 555;
            var offer1 = new Offer { Id = 111, SupplierId = 2, SourceAddress = "Addr1", DestAddress = "Addr2", Price = 1.1 };
            var offer2 = new Offer { Id = 222, SupplierId = 3, SourceAddress = "Addr1", DestAddress = "Addr2", Price = 1.2 };
            var offers = new List<Offer> { offer1, offer2 };
            var offersRepositoryMock = ServiceUnitTestsHelper.SetupRepository<Offer>(UnityContainerMock);
            offersRepositoryMock.SetupGet(p => p.All).Returns(offers.AsQueryable);
            offersRepositoryMock.Setup(p => p.Find(It.Is<int>(x => x == offer1.Id))).Returns(offer1);
            offersRepositoryMock.Setup(p => p.Find(It.Is<int>(x => x == offer2.Id))).Returns(offer2);
            offersRepositoryMock.Setup(p => p.Find(It.Is<int>(x => x == id))).Returns(default(Offer));

            // Act
            var service = new OffersAppService(UnityContainerMock.Object);
            var result = service.Get(id);

            // Assert
            UnityContainerMock.Verify(p =>
                    p.Resolve(
                        It.Is<Type>(x => x == typeof(IRepository<Offer>)),
                        It.Is<string>(x => x == default),
                        It.Is<ResolverOverride[]>(x => x.Length == 0)),
                Times.Once);

            offersRepositoryMock.VerifyGet(p => p.All, Times.Never);
            offersRepositoryMock.Verify(p => p.Find(It.Is<int>(x => x == offer1.Id)), Times.Never);
            offersRepositoryMock.Verify(p => p.Find(It.Is<int>(x => x == offer2.Id)), Times.Never);
            offersRepositoryMock.Verify(p => p.Find(It.Is<int>(x => x == id)), Times.Once);
            Assert.IsNull(result);
        }

        [TestCase(2, "Addr1", "Addr3")]
        [TestCase(2, "Addr3", "Addr2")]
        [TestCase(4, "Addr1", "Addr2")]
        public void AddOrUpdate_Add(int supplierId, string sourceAddress, string destAddress)
        {
            // Arrange
            var offers = new List<Offer>
            {
                new Offer { Id = 1, SupplierId = 2, SourceAddress = "Addr1", DestAddress = "Addr2", Price = 1.1 },
                new Offer { Id = 2, SupplierId = 3, SourceAddress = "Addr1", DestAddress = "Addr2", Price = 1.2 },
            };

            var newOffer = new Offer { SupplierId = supplierId, SourceAddress = sourceAddress, DestAddress = destAddress, Price = 2.0 };
            var offersRepositoryMock = ServiceUnitTestsHelper.SetupRepository<Offer>(UnityContainerMock);
            offersRepositoryMock.SetupGet(p => p.All).Returns(offers.AsQueryable);
            offersRepositoryMock.Setup(p => p.Create(It.Is<Offer>(x => x == newOffer))).Returns(3);
            offersRepositoryMock.Setup(p => p.Update(It.IsAny<Offer>()));

            // Act
            var service = new OffersAppService(UnityContainerMock.Object);
            var result = service.AddOrUpdate(newOffer);

            // Assert
            UnityContainerMock.Verify(p =>
                    p.Resolve(
                        It.Is<Type>(x => x == typeof(IRepository<Offer>)),
                        It.Is<string>(x => x == default),
                        It.Is<ResolverOverride[]>(x => x.Length == 0)),
                Times.Once);

            offersRepositoryMock.VerifyGet(p => p.All, Times.Once);
            offersRepositoryMock.Verify(p => p.Create(It.Is<Offer>(x => x == newOffer)), Times.Once);
            offersRepositoryMock.Verify(p => p.Update(It.IsAny<Offer>()), Times.Never);
            Assert.AreSame(newOffer, result);
        }

        [TestCase(2, "Addr1", "Addr2", 2.1)]
        [TestCase(3, "Addr2", "Addr3", 2.2)]
        public void AddOrUpdate_Update(int supplierId, string sourceAddress, string destAddress, double newPrice)
        {
            // Arrange
            var offers = new List<Offer>
            {
                new Offer { Id = 1, SupplierId = 2, SourceAddress = "Addr1", DestAddress = "Addr2", Price = 1.1 },
                new Offer { Id = 2, SupplierId = 3, SourceAddress = "Addr2", DestAddress = "Addr3", Price = 1.2 },
            };

            var newOffer = new Offer { SupplierId = supplierId, SourceAddress = sourceAddress, DestAddress = destAddress, Price = newPrice };
            var offersRepositoryMock = ServiceUnitTestsHelper.SetupRepository<Offer>(UnityContainerMock);
            offersRepositoryMock.SetupGet(p => p.All).Returns(offers.AsQueryable);
            offersRepositoryMock.Setup(p => p.Create(It.IsAny<Offer>())).Returns(-1);
            offersRepositoryMock.Setup(p => p.Update(It.IsAny<Offer>()));

            // Act
            var service = new OffersAppService(UnityContainerMock.Object);
            var result = service.AddOrUpdate(newOffer);

            // Assert
            UnityContainerMock.Verify(p =>
                    p.Resolve(
                        It.Is<Type>(x => x == typeof(IRepository<Offer>)),
                        It.Is<string>(x => x == default),
                        It.Is<ResolverOverride[]>(x => x.Length == 0)),
                Times.Once);

            offersRepositoryMock.VerifyGet(p => p.All, Times.Once);
            offersRepositoryMock.Verify(p => p.Create(It.IsAny<Offer>()), Times.Never);
            offersRepositoryMock.Verify(p => p.Update(It.IsAny<Offer>()), Times.Once);
            Assert.AreNotSame(newOffer, result);
            Assert.AreEqual(newOffer.Price, result.Price, 1E-6);
        }

        [TestCase("Addr1", "Addr2", new[] { 2.0, 3.0, 4.0, }, 9.9)]
        [TestCase("Addr1", "Addr3", new[] { 2.0, 3.0, 4.0, }, 19.8)]
        [TestCase("Addr2", "Addr3", new[] { 2.0, 3.0, 4.0, }, 27.0)]
        public void Calculate(string source, string destination, double[] packages, double expectedResult)
        {
            // Arrange
            var offers = new List<Offer>
            {
                new Offer { Id = 1, SupplierId = 2, SourceAddress = "Addr1", DestAddress = "Addr2", Price = 1.2 },
                new Offer { Id = 2, SupplierId = 3, SourceAddress = "Addr1", DestAddress = "Addr2", Price = 1.1 },
                new Offer { Id = 3, SupplierId = 4, SourceAddress = "Addr1", DestAddress = "Addr2", Price = 1.3 },
                new Offer { Id = 4, SupplierId = 2, SourceAddress = "Addr1", DestAddress = "Addr3", Price = 2.4 },
                new Offer { Id = 5, SupplierId = 3, SourceAddress = "Addr1", DestAddress = "Addr3", Price = 2.3 },
                new Offer { Id = 6, SupplierId = 4, SourceAddress = "Addr1", DestAddress = "Addr3", Price = 2.2 },
                new Offer { Id = 7, SupplierId = 2, SourceAddress = "Addr2", DestAddress = "Addr3", Price = 3.3 },
                new Offer { Id = 8, SupplierId = 3, SourceAddress = "Addr2", DestAddress = "Addr3", Price = 3.4 },
                new Offer { Id = 9, SupplierId = 4, SourceAddress = "Addr2", DestAddress = "Addr3", Price = 3.0 },
            };

            var offersRepositoryMock = ServiceUnitTestsHelper.SetupRepository<Offer>(UnityContainerMock);
            offersRepositoryMock.SetupGet(p => p.All).Returns(offers.AsQueryable);

            // Act
            var service = new OffersAppService(UnityContainerMock.Object);
            var result = service.Calculate(source, destination, packages);

            // Assert
            UnityContainerMock.Verify(p =>
                    p.Resolve(
                        It.Is<Type>(x => x == typeof(IRepository<Offer>)),
                        It.Is<string>(x => x == default),
                        It.Is<ResolverOverride[]>(x => x.Length == 0)),
                Times.Once);

            offersRepositoryMock.VerifyGet(p => p.All, Times.Once);
            Assert.AreEqual(expectedResult, result, 1E-6);
        }

        [TestCase("Addr1", "Invalid", new[] { 2.0, 3.0, 4.0, })]
        [TestCase("Invalid", "Addr3", new[] { 2.0, 3.0, 4.0, })]
        public void Calculate_NotFound(string source, string destination, double[] packages)
        {
            // Arrange
            var offers = new List<Offer>
            {
                new Offer { Id = 1, SupplierId = 2, SourceAddress = "Addr1", DestAddress = "Addr2", Price = 1.2 },
                new Offer { Id = 2, SupplierId = 3, SourceAddress = "Addr1", DestAddress = "Addr2", Price = 1.1 },
                new Offer { Id = 3, SupplierId = 4, SourceAddress = "Addr1", DestAddress = "Addr2", Price = 1.3 },
                new Offer { Id = 4, SupplierId = 2, SourceAddress = "Addr1", DestAddress = "Addr3", Price = 2.4 },
                new Offer { Id = 5, SupplierId = 3, SourceAddress = "Addr1", DestAddress = "Addr3", Price = 2.3 },
                new Offer { Id = 6, SupplierId = 4, SourceAddress = "Addr1", DestAddress = "Addr3", Price = 2.2 },
                new Offer { Id = 7, SupplierId = 2, SourceAddress = "Addr2", DestAddress = "Addr3", Price = 3.3 },
                new Offer { Id = 8, SupplierId = 3, SourceAddress = "Addr2", DestAddress = "Addr3", Price = 3.4 },
                new Offer { Id = 9, SupplierId = 4, SourceAddress = "Addr2", DestAddress = "Addr3", Price = 3.0 },
            };

            var offersRepositoryMock = ServiceUnitTestsHelper.SetupRepository<Offer>(UnityContainerMock);
            offersRepositoryMock.SetupGet(p => p.All).Returns(offers.AsQueryable);

            // Act
            var service = new OffersAppService(UnityContainerMock.Object);
            var result = service.Calculate(source, destination, packages);

            // Assert
            UnityContainerMock.Verify(p =>
                    p.Resolve(
                        It.Is<Type>(x => x == typeof(IRepository<Offer>)),
                        It.Is<string>(x => x == default),
                        It.Is<ResolverOverride[]>(x => x.Length == 0)),
                Times.Once);

            offersRepositoryMock.VerifyGet(p => p.All, Times.Once);
            Assert.IsNaN(result);
        }
    }
}
