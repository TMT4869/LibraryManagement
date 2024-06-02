using System.Linq.Expressions;

namespace FA.LibraryManagement.Core.Infrastructers;

/// <summary>
///     The base repository interface
/// </summary>
public interface IBaseRepository<TEntity> where TEntity : class
{
    /// <summary>
    ///     Change state of entity to added
    /// </summary>
    /// <param name="entity"></param>
    void Create(TEntity entity);

    /// <summary>
    ///     Change state of entities to added
    /// </summary>
    /// <param name="entities"></param>
    void CreateRange(List<TEntity> entities);

    /// <summary>
    ///     Change state of entity to deleted
    /// </summary>
    /// <param name="entity"></param>
    void Delete(TEntity entity);

    /// <summary>
    ///     Change state of entity to deleted
    /// </summary>
    /// <param name="entity"></param>
    void Delete(params object[] ids);

    /// <summary>
    ///     Change state of entity to modified
    /// </summary>
    /// <param name="entity"></param>
    void Update(TEntity entity);

    /// <summary>
    ///     Change state of entities to modified
    /// </summary>
    /// <param name="entity"></param>
    void UpdateRange(List<TEntity> entities);

    /// <summary>
    ///     Get all <paramref name="TEntity"></paramref> from database
    /// </summary>
    /// <returns></returns>
    IEnumerable<TEntity> GetAll();

    /// <summary>
    ///     Gets the by id using the specified primary key
    /// </summary>
    /// <param name="primaryKey">The primary key</param>
    /// <returns>The entity</returns>
    TEntity GetById(params object[] primaryKey);

    /// <summary>
    ///     Finds the predicate
    /// </summary>
    /// <param name="predicate">The predicate</param>
    /// <returns>An enumerable of t entity</returns>
    IEnumerable<TEntity> Find(Func<TEntity, bool> predicate);

    /// <summary>
    /// Gets the all using the specified filter
    /// </summary>
    /// <param name="filter">The filter</param>
    /// <param name="includeProperties">The include properties</param>
    /// <returns>An enumerable of t entity</returns>
    IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null, string? includeProperties = null);

    /// <summary>
    /// Gets the filter
    /// </summary>
    /// <param name="filter">The filter</param>
    /// <param name="includeProperties">The include properties</param>
    /// <param name="tracked">The tracked</param>
    /// <returns>The entity</returns>
    TEntity Get(Expression<Func<TEntity, bool>> filter, string? includeProperties = null, bool tracked = false);
}