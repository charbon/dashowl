using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace DashOwl.Models
{
    public abstract class GenericRepository<C, T> :
     IGenericRepository<T>
        where T : class
        where C : DbContext, new()
    {
        private C _entities = new C();
        public C Context
        {

            get { return _entities; }
            set { _entities = value; }
        }

        public virtual RepositoryQuery<C, T> Query()
        {
            var repositoryGetFluentHelper =
                new RepositoryQuery<C, T>(this);

            return repositoryGetFluentHelper;
        }

        internal IQueryable<T> Get(
        Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>,
            IOrderedQueryable<T>> orderBy = null,
        List<Expression<Func<T, object>>>
            includeProperties = null,
        int? page = null,
        int? pageSize = null)
        {
            IQueryable<T> query = _entities.Set<T>();

            if (includeProperties != null)
                includeProperties.ForEach(i => { query = query.Include(i); });

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            if (page != null && pageSize != null)
                query = query
                    .Skip((page.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);

            return query;
        }

        public virtual IQueryable<T> GetAll()
        {
            IQueryable<T> query = _entities.Set<T>();
            return query;
        }

        public IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _entities.Set<T>().Where(predicate);
            return query;
        }

        public virtual void Add(T entity)
        {
            _entities.Set<T>().Add(entity);
        }

        public virtual void Delete(T entity)
        {
            _entities.Set<T>().Remove(entity);
        }

        public virtual void Edit(T entity)
        {
            _entities.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Save()
        {
            _entities.SaveChanges();
        }
    }
}