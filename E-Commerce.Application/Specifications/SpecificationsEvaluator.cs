using E_Commerce.Domain.Common;
using E_Commerce.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications
{
    public static class SpecificationsEvaluator
    {
        public static IQueryable<TEntity>CreateQuery<TEntity , TKey>(IQueryable<TEntity> inputQuery , ISpecifications<TEntity , TKey> Spec) where TEntity : BaseEntity<TKey>
        {
            var query = inputQuery;
            if (Spec.Criteria != null)
            {
                query = query.Where(Spec.Criteria);
            }
            if (Spec.OrderBy != null)
            {
                query = query.OrderBy(Spec.OrderBy);
            }
            else if (Spec.OrderByDescending != null) { 
            query = query.OrderByDescending(Spec.OrderByDescending);
            }
            if (Spec.IncludeExpression.Any())
            {
                query = Spec.IncludeExpression.Aggregate(query ,(current , NextStep) => current.Include(NextStep) );
            }

            if (Spec.IsPaginated)
            {
                query = query.Skip(Spec.Skip).Take(Spec.Take);
            }
            return query;
        }
    }
}
