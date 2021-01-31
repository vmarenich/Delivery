using System.Linq;
using WebApplication1.Domain;

namespace WebApplication1.DAL
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
