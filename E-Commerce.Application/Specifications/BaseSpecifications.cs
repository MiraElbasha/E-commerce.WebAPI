using E_Commerce.Domain.Common;
using E_Commerce.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications
{
    public class BaseSpecifications<TEntity, TKey> : ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public ICollection<Expression<Func<TEntity, object>>> IncludeExpression { get; } = [];

        public Expression<Func<TEntity, bool>> Criteria { get; private set; }

        public Expression<Func<TEntity, object>> OrderBy{ get; private set; }
        protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        public Expression<Func<TEntity, object>> OrderByDescending { get; private set; }

        public int Take {  get; private set; }

        public int Skip { get; private set; }

        public bool IsPaginated  { get; private set; }

        protected void ApplyPagination (int PageSize , int PageIndex)
        {
            IsPaginated = true;
            Take = PageSize;
            Skip =(PageIndex -1) * PageSize;
        }

        protected void AddOrderByDesc(Expression<Func<TEntity, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }
        protected BaseSpecifications(Expression<Func<TEntity , bool>> criteria)
        {
            Criteria = criteria;
        }

        protected void AddInclude(Expression<Func<TEntity , object>> include)
        {
            IncludeExpression.Add(include);
        }
    }
}
