﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Repository.Interfaces;

public interface IIncludableRepository<TEntity, TProp>
{
    protected IIncludableQueryable<TEntity, TProp> Queryable { get; }

    public IIncludableRepository<TEntity, TNew> Include<TNew>(Expression<Func<TEntity, TNew>> property);
    public IThenIncludableRepository<TEntity, TNew> Include<TNew>(Expression<Func<TEntity, IEnumerable<TNew>>> property);
    public IThenIncludableRepository<TEntity, TNew> Include<TNew>(Expression<Func<TEntity, ICollection<TNew>>> property);

    public Task<TEntity?> GetByAsync(Expression<Func<TEntity, bool>> filter);
    public IAsyncEnumerable<TEntity> GetAllAsync();
    public IAsyncEnumerable<TEntity> GetAllWhereAsync(Expression<Func<TEntity, bool>> filter);
    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
}
