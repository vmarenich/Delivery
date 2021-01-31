using Unity;
using DeliveryWebApi.DAL;
using DeliveryWebApi.Domain;

namespace DeliveryWebApi.Services
{
    public abstract class AppServiceBase
    {
        protected AppServiceBase(IUnityContainer unityContainer)
        {
            UnityContainer = unityContainer;
        }

        private IUnityContainer UnityContainer { get; }

        protected IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : EntityBase
        {
            return UnityContainer.Resolve<IRepository<TEntity>>();
        }
    }
}