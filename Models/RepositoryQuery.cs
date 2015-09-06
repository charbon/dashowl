using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace DashOwl.Models
{
    public sealed class RepositoryQuery<C,T> 
        where T : class
        where C : DbContext, new()
    {
        private readonly List<Expression<Func<T, object>>>
            _includeProperties;

        private readonly GenericRepository<C, T> _repository;
        private Expression<Func<T, bool>> _filter;
        private Func<IQueryable<T>,
            IOrderedQueryable<T>> _orderByQuerable;
        private int? _page;
        private int? _pageSize;

        public RepositoryQuery(GenericRepository<C, T> repository)
        {
            _repository = repository;
            _includeProperties =
                new List<Expression<Func<T, object>>>();
        }

        public RepositoryQuery<C, T> Filter(
            Expression<Func<T, bool>> filter)
        {
            _filter = filter;
            return this;
        }

        public RepositoryQuery<C, T> OrderBy(
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        {
            _orderByQuerable = orderBy;
            return this;
        }

        public RepositoryQuery<C, T> Include(
            Expression<Func<T, object>> expression)
        {
            _includeProperties.Add(expression);
            return this;
        }

        public IEnumerable<T> GetPage(
            int page, int pageSize, out int totalCount)
        {
            _page = page;
            _pageSize = pageSize;
            totalCount = _repository.Get(_filter).Count();

            return _repository.Get(
                _filter,
                _orderByQuerable, _includeProperties, _page, _pageSize);
        }

        public IEnumerable<T> Get()
        {
            return _repository.Get(
                _filter,
                _orderByQuerable, _includeProperties, _page, _pageSize);
        }
    }
}