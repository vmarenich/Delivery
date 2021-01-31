using System.Linq;
using DeliveryWebApi.Domain;

namespace DeliveryWebApi.DAL
{
    public interface IRepository<TEntity>
        where TEntity : EntityBase
    {
        IQueryable<TEntity> All { get; }

        TEntity Find(int id);

        int Create(TEntity entity);

        void Update(TEntity entity);

        void Delete(int id);
    }
}
