using FA.LibraryManagement.Core.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FA.LibraryManagement.Core.Infrastructers;

/// <summary>
///     The base repository class
/// </summary>
/// <seealso cref="IBaseRepository{TEntity}" />
public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : class
{
    /// <summary>
    ///     The context
    /// </summary>
    protected readonly LibraryManagementContext Context;

    /// <summary>
    ///     The db set
    /// </summary>
    protected DbSet<TEntity> DbSet;

    /// <summary>
    ///     Initializes a new instance of the <see cref="BaseRepository" /> class
    /// </summary>
    /// <param name="context">The context</param>
    protected BaseRepository(LibraryManagementContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }

    /// <summary>
    ///     Creates the entity
    /// </summary>
    /// <param name="entity">The entity</param>
    public void Create(TEntity entity)
    {
        DbSet.Add(entity);
        //Context.Entry<TEntity>(entity).State = EntityState.Added;
    }

    /// <summary>
    ///     Creates the range using the specified entities
    /// </summary>
    /// <param name="entities">The entities</param>
    public void CreateRange(List<TEntity> entities)
    {
        DbSet.AddRange(entities);
    }

    /// <summary>
    ///     Deletes the entity
    /// </summary>
    /// <param name="entity">The entity</param>
    public void Delete(TEntity entity)
    {
        DbSet.Remove(entity);
        // Context.Entry<TEntity>(entity).State = EntityState.Deleted;
    }

    /// <summary>
    /// Deletes the ids
    /// </summary>
    /// <param name="ids">The ids</param>
    /// <exception cref="ArgumentException">{string.Join(";", ids)} not exist in the {typeof(TEntity).Name} table</exception>
    public void Delete(params object[] ids)
    {
        var entity = DbSet.Find(ids);
        if (entity == null)
            throw new ArgumentException($"{string.Join(";", ids)} not exist in the {typeof(TEntity).Name} table");
        DbSet.Remove(entity);
    }

    /// <summary>
    ///     Finds the predicate
    /// </summary>
    /// <param name="predicate">The predicate</param>
    /// <returns>An enumerable of t entity</returns>
    public IEnumerable<TEntity> Find(Func<TEntity, bool> predicate)
    {
        return DbSet.Where(predicate);
    }

    /// <summary>
    /// Gets the all using the specified filter
    /// </summary>
    /// <param name="filter">The filter</param>
    /// <param name="includeProperties">The include properties</param>
    /// <returns>An enumerable of t entity</returns>
    public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null, string? includeProperties = null)
    {
        IQueryable<TEntity> query = DbSet;
        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProp in includeProperties
                         .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
        }

        return query.ToList();
    }


    /// <summary>
    /// Gets the filter
    /// </summary>
    /// <param name="filter">The filter</param>
    /// <param name="includeProperties">The include properties</param>
    /// <param name="tracked">The tracked</param>
    /// <returns>The entity</returns>
    public TEntity Get(Expression<Func<TEntity, bool>> filter, string? includeProperties = null, bool tracked = false)
    {
        IQueryable<TEntity> query;
        if (tracked)
        {
            query = DbSet;
        }
        else
        {
            query = DbSet.AsNoTracking();
        }

        query = query.Where(filter);
        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProp in includeProperties
                         .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
        }

        return query.FirstOrDefault();
    }


    /// <summary>
    ///     Gets the all
    /// </summary>
    /// <returns>The db set</returns>
    public IEnumerable<TEntity> GetAll()
    {
        return DbSet;
    }

    /// <summary>
    ///     Gets the by id using the specified primary key
    /// </summary>
    /// <param name="primaryKey">The primary key</param>
    /// <returns>The entity</returns>
    public TEntity GetById(params object[] primaryKey)
    {
        return DbSet.Find(primaryKey);
    }

    /// <summary>
    ///     Updates the entity
    /// </summary>
    /// <param name="entity">The entity</param>
    public void Update(TEntity entity)
    {
        DbSet.Update(entity);
        // Context.Entry<TEntity>(entity).State = EntityState.Modified;
    }

    /// <summary>
    ///     Updates the range using the specified entities
    /// </summary>
    /// <param name="entities">The entities</param>
    public void UpdateRange(List<TEntity> entities)
    {
        DbSet.UpdateRange(entities);
    }
}