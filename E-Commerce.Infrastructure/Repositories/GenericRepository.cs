using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Common;
using E_Commerce.Domain.Contracts;
using E_Commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories
{
    public class GenericRepository<TEntity, TKey>(StoreDbContext dbContext) : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        //go to dbContext , the DBset<Entity> Add this entity
        public void Add(TEntity entity) => dbContext.Set<TEntity>().Add(entity);
        
        //go to dbContext , the DBset<Entity> Remove this entity
        public void Remove(TEntity entity) => dbContext.Set<TEntity>().Remove(entity);

        //go to dbContext , the DBset<Entity> update this entity
        public void Update(TEntity entity) => dbContext.Set<TEntity>().Update(entity);

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct = default) => await dbContext.Set<TEntity>().AsNoTracking().ToListAsync(ct);


        public async Task<TEntity?> GetByIdAsync(TKey Id, CancellationToken ct = default) => await dbContext.Set<TEntity>().FindAsync([Id!], ct).AsTask();

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(ISpecifications<TEntity, TKey> Spec, CancellationToken ct = default)
        {
            var query = SpecificationsEvaluator.CreateQuery(dbContext.Set<TEntity>(), Spec);
            return await query.ToListAsync(ct);
        }

        public async Task<TEntity?> GetByIdAsync(ISpecifications<TEntity, TKey> Spec, CancellationToken ct = default)
        {
            var query = SpecificationsEvaluator.CreateQuery(dbContext.Set<TEntity>(), Spec);
            return await query.FirstOrDefaultAsync(ct);
        }

        public Task<int> CountAsync(ISpecifications<TEntity, TKey> Spec, CancellationToken ct = default)
        {
            return SpecificationsEvaluator.CreateQuery(dbContext.Set<TEntity>() , Spec).CountAsync(ct);
        }
    }
}
