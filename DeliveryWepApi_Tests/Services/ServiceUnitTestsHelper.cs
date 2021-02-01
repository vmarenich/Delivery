using System;
using DeliveryWebApi.DAL;
using DeliveryWebApi.Domain;
using Moq;
using Unity;
using Unity.Resolution;

namespace DeliveryWepApi_Tests.Services
{
    public static class ServiceUnitTestsHelper
    {
        public static Mock<IRepository<TEntity>> SetupRepository<TEntity>(Mock<IUnityContainer> unityContainerMock)
            where TEntity : EntityBase
        {
            var repositoryMock = new Mock<IRepository<TEntity>>();
            var repository = repositoryMock.Object;
            unityContainerMock.Setup(p =>
                    p.Resolve(
                        It.Is<Type>(x => x == typeof(IRepository<TEntity>)),
                        It.Is<string>(x => x == default),
                        It.Is<ResolverOverride[]>(x => x.Length == 0)))
                .Returns(repository);

            return repositoryMock;
        }
    }
}