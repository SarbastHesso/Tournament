using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Interfaces;
using Tournament.Core.Request;
using Tournament.Data.Data;
using Tournament.Shared.Dto;


namespace Tournament.Data.Repositories;

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected TournamentApiContext Context { get; }
    protected DbSet<T> DbSet { get; }
    public RepositoryBase(TournamentApiContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    public void Create(T entity)
    {
        DbSet.Add(entity);
    }

    public void Delete(T entity)
    {
        DbSet.Remove(entity);
    }

    public IQueryable<T> FindAll(bool trackChanges = false)
    {
        return trackChanges ? DbSet : DbSet.AsNoTracking();
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges=false)
    {
        return trackChanges ? DbSet.Where(expression) : DbSet.Where(expression).AsNoTracking();
    }

    public void Update(T entity)
    {
        DbSet.Update(entity);
    }

    public async Task<PagedResult<T>> GetPagedAsync(IQueryable<T> query, int page, int pageSize)
    {
        var totalItems = await query.CountAsync();
        var items = await query
            .Skip((page -1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<T>
        {
            Items = items,
            TotalItems = totalItems,
            PageSize = pageSize,
            CurrentPage = page
        };
    }
}
