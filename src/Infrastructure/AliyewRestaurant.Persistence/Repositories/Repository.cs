﻿using AliyewRestaurant.Application.Abstracts.Repositories;
using AliyewRestaurant.Domain.Entites;
using AliyewRestaurant.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AliyewRestaurant.Persistence.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity, new()
{
    private readonly AliyewRestaurantDbContext _context;

    private readonly DbSet<T> Table;

    public Repository(AliyewRestaurantDbContext context)
    {
        _context = context;
        Table = _context.Set<T>();
    }

    public async Task AddAsync(T entity)
    {
        entity.CreatedAt = DateTime.Now;
        await Table.AddAsync(entity);
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await Table.ToListAsync();
    }

    public void Update(T entity)
    {
        Table.Update(entity);
    }

    public void Delete(T entity)
    {
        Table.Remove(entity);
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await Table.FindAsync(id);
    }

    public IQueryable<T> GetByFiltered(Expression<Func<T, bool>> predicate,
        Expression<Func<T, object>>[]? include = null,
        bool isTracking = false)
    {
        IQueryable<T> query = Table;
        if (predicate is not null)
            query = query.Where(predicate);

        if (include is not null)
        {
            foreach (var includeExpression in include)
                query = query.Include(includeExpression);
        }

        if (!isTracking)
            return query.AsNoTracking();

        return query;

    }

    public IQueryable<T> GetAll(bool isTracking = false)
    {
        if (!isTracking)
            return Table.AsNoTracking();
        return Table;
    }


    public IQueryable<T> GetAllFiltered(Expression<Func<T, bool>> predicate,
        Expression<Func<T, object>>[]? include = null,
        Expression<Func<T, object>>? orderby = null,
        bool isOrderByAsc = true,
        bool isTracking = false)
    {
        IQueryable<T> query = Table;
        if (predicate is not null)
            query = query.Where(predicate);

        if (include is not null)
        {
            foreach (var includeExpression in include)
                query = query.Include(includeExpression);
        }

        if (orderby is not null)
        {
            if (isOrderByAsc)
                query = query.OrderBy(orderby);
            else
                query = query.OrderByDescending(orderby);
        }

        if (!isTracking)
            return query.AsNoTracking();

        return query;
    }

    public async Task SaveChangeAsync()
    {
        await _context.SaveChangesAsync();
    }
}