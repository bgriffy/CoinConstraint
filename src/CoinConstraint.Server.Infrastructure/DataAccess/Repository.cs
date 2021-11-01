﻿using CoinConstraint.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CoinConstraint.Server.Infrastructure.DataAccess
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> _dbset;

        public Repository(DbContext dbContext)
        {
            _dbset = dbContext.Set<T>();
        }

        public void Add(T entity)
        {
            _dbset.Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _dbset.AddRange(entities);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbset.Where(predicate);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbset.ToList();
        }

        public void Remove(T entity)
        {
            _dbset.Remove(entity);
        }

        public void RemoveAll()
        {
            _dbset.RemoveRange();
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbset.RemoveRange(entities);
        }

        public void Update(T entity)
        {
            _dbset.Update(entity);
        }
    }
}
