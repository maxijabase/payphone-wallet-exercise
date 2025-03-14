using Microsoft.EntityFrameworkCore;
using PayphoneWallet.Domain.Entities;
using PayphoneWallet.Infrastructure.Context;
using PayphoneWallet.Infrastructure.Interfaces;

namespace PayphoneWallet.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null)
        {
            return false;
        }
        _dbSet.Remove(entity);
        int aff = await _context.SaveChangesAsync();
        return aff > 0;
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }
    public async Task<bool> UpdateAsync(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        // Don't modify 'createdAt'
        _context.Entry(entity).Property(x => x.CreatedAt).IsModified = false;
        int aff = await _context.SaveChangesAsync();
        return aff > 0;
    }
}
