namespace BetTracker.Data.Repositories;

/// <summary>
/// Generic repository interface for standard CRUD operations.
/// </summary>
/// <typeparam name="T">The entity type</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Get an entity by its primary key.
    /// </summary>
    Task<T?> GetByIdAsync(int id);

    /// <summary>
    /// Get all entities.
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Add a new entity.
    /// </summary>
    Task AddAsync(T entity);

    /// <summary>
    /// Update an existing entity.
    /// </summary>
    Task UpdateAsync(T entity);

    /// <summary>
    /// Delete an entity by its primary key.
    /// </summary>
    Task DeleteAsync(int id);

    /// <summary>
    /// Save changes to the database.
    /// </summary>
    Task SaveChangesAsync();
}
