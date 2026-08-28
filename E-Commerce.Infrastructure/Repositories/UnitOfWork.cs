using E_Commerce.Domain.Common;
using E_Commerce.Domain.Contracts;
using E_Commerce.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories
{
    //use primary constructor to inject DdContext
    public class UnitOfWork(StoreDbContext dbContext) : IUnitOfWork
    {
        //create an empty Dictionary to store Repositories
        private readonly Dictionary<string , object> _repositories = [];


        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            //get/store entity name in typeName Variable --> Product/Brand/Type
           var TypeName = typeof(TEntity).Name;
            //if this repo exists (tryGetValue)
            if (_repositories.TryGetValue(TypeName , out object? value))
                //return this repo
                return (IGenericRepository<TEntity, TKey>)value;
            //if it's not create a repo for this entity
            var Repo = new GenericRepository<TEntity, TKey>(dbContext);
            //then store it in the dictionary
            _repositories[TypeName] = Repo;
            return Repo;
           
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => dbContext.SaveChangesAsync(ct);
    }
}
