using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

        public void Setup(IEnumerable<TEntity> entities)
        {
            _all.Clear();
            foreach (var entity in entities)
            {
                _all.TryAdd(entity.Id, entity);
                _idCounter = Math.Max(entity.Id, _idCounter);
            }
        }

        public IQueryable<TEntity> All => _all.Values.AsQueryable();

        public TEntity Find(int id)
        {
            return _all.TryGetValue(id, out var result) ? result : null;
        }

        public int Create(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entity.Id = Interlocked.Increment(ref _idCounter);
            _all.TryAdd(entity.Id, entity);
            return entity.Id;
        }

        public void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (_all.TryGetValue(entity.Id, out var persistent))
            {
                _all.TryUpdate(entity.Id, entity, persistent);
            }
        }

        public void Delete(int id)
        {
            _all.TryRemove(id, out _);
        }
    }
}