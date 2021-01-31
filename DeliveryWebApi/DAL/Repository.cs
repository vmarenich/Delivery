using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using DeliveryWebApi.Domain;

namespace DeliveryWebApi.DAL
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : EntityBase
    {
        private readonly ConcurrentDictionary<int, TEntity> _all = new ConcurrentDictionary<int, TEntity>();

        private int _idCounter;

        public IQueryable<TEntity> All => _all.Values.AsQueryable();

        public TEntity Find(int id)
        {
            return _all.TryGetValue(id, out var result) ? result : null;
        }

        public int Create(TEntity entity)
        {
            entity.Id = Interlocked.Increment(ref _idCounter);
            _all.TryAdd(entity.Id, entity);
            return entity.Id;
        }

        public void Update(TEntity entity)
        {
            _all.TryUpdate(entity.Id, entity, entity);
        }

        public void Delete(int id)
        {
            _all.TryRemove(id, out _);
        }
    }
}