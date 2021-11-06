﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CoinConstraint.Domain.Interfaces
{
    public interface IServersideRepository<T> : IRepository<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void AddRange(IEnumerable<T> entities);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        void RemoveAll();
        Task SaveChangesAsync();
    }
}
