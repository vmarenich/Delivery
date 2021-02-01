using System;
using System.Collections.Generic;
using System.Linq;
using DeliveryWebApi.DAL;
using DeliveryWebApi.Domain;
using NUnit.Framework;

namespace DeliveryWepApi_Tests.DAL
{
    [TestFixture(Category = "UnitTests")]
    public class Repository_UnitTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void All()
        {
            // Arrange
            var entity1 = new Supplier { Id = 1, Name = "Supplier1" };
            var entity2 = new Supplier { Id = 2, Name = "Supplier2" };
            var entities = new List<Supplier> { entity1, entity2 };
            var repository = new Repository<Supplier>();
            repository.Setup(entities);

            // Act
            var result = repository.All.ToList();

            // Assert
            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(entities, result);
        }

        [Test]
        public void Find_id()
        {
            // Arrange
            var entity1 = new Supplier { Id = 1, Name = "Supplier1" };
            var entity2 = new Supplier { Id = 2, Name = "Supplier2" };
            var entities = new List<Supplier> { entity1, entity2 };
            var repository = new Repository<Supplier>();
            repository.Setup(entities);

            // Act
            var result = repository.Find(2);

            // Assert
            Assert.AreEqual(entity2, result);
        }

        [Test]
        public void Find_id_NotFound()
        {
            // Arrange
            var id = 555;
            var entity1 = new Supplier { Id = 1, Name = "Supplier1" };
            var entity2 = new Supplier { Id = 2, Name = "Supplier2" };
            var entities = new List<Supplier> { entity1, entity2 };
            var repository = new Repository<Supplier>();
            repository.Setup(entities);

            // Act
            var result = repository.Find(id);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void Create()
        {
            // Arrange
            var entity1 = new Supplier { Id = 1, Name = "Supplier1" };
            var entity2 = new Supplier { Id = 2, Name = "Supplier2" };
            var entities = new List<Supplier> { entity1, entity2 };
            var repository = new Repository<Supplier>();
            repository.Setup(entities);
            var target = new Supplier { Name = "NewSupplier" };

            // Act
            var result = repository.Create(target);

            // Assert
            var all = repository.All.ToList();
            CollectionAssert.IsNotEmpty(all);
            Assert.AreEqual(3, all.Count);
            Assert.AreEqual(3, result);
            Assert.AreEqual(3, target.Id);
            Assert.AreEqual(3, all[2].Id);
            Assert.AreEqual("NewSupplier", all[2].Name);
        }

        [Test]
        public void Create_Null()
        {
            // Arrange
            var repository = new Repository<Supplier>();

            // Act
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    repository.Create(null);
                });

            // Assert
            var all = repository.All.ToList();
            CollectionAssert.IsEmpty(all);
        }

        [Test]
        public void Update()
        {
            // Arrange
            var entity1 = new Supplier { Id = 1, Name = "Supplier1" };
            var entity2 = new Supplier { Id = 2, Name = "Supplier2" };
            var entities = new List<Supplier> { entity1, entity2 };
            var repository = new Repository<Supplier>();
            repository.Setup(entities);
            var target = new Supplier { Id = 2, Name = "NewSupplier" };

            // Act
            repository.Update(target);

            // Assert
            var all = repository.All.ToList();
            CollectionAssert.IsNotEmpty(all);
            Assert.AreEqual(2, all.Count);
            Assert.AreEqual("NewSupplier", all[1].Name);
        }

        [Test]
        public void Update_Null()
        {
            // Arrange
            var entity1 = new Supplier { Id = 1, Name = "Supplier1" };
            var entity2 = new Supplier { Id = 2, Name = "Supplier2" };
            var entities = new List<Supplier> { entity1, entity2 };
            var repository = new Repository<Supplier>();
            repository.Setup(entities);

            // Act
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    repository.Update(null);
                });

            // Assert
            var all = repository.All.ToList();
            CollectionAssert.IsNotEmpty(all);
            Assert.AreEqual(2, all.Count);
            Assert.AreEqual("Supplier1", all[0].Name);
            Assert.AreEqual("Supplier2", all[1].Name);
        }

        [Test]
        public void Delete()
        {
            // Arrange
            var entity1 = new Supplier { Id = 1, Name = "Supplier1" };
            var entity2 = new Supplier { Id = 2, Name = "Supplier2" };
            var entities = new List<Supplier> { entity1, entity2 };
            var repository = new Repository<Supplier>();
            repository.Setup(entities);

            // Act
            repository.Delete(1);

            // Assert
            var all = repository.All.ToList();
            CollectionAssert.IsNotEmpty(all);
            Assert.AreEqual(1, all.Count);
            Assert.AreEqual(2, all[0].Id);
            Assert.AreEqual("Supplier2", all[0].Name);
        }
    }
}
