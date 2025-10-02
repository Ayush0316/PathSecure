namespace Infrastructure.Database;

/// <summary>
/// Represents a repository interface for performing CRUD operations on entities of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Returns an <see cref="IQueryable{T}"/> for querying entities of type <typeparamref name="T"/>.
    /// It automatically filters out entities marked as deleted if <typeparamref name="T"/> is or derives from <see cref="BaseEntity"/>.
    /// </summary>
    IQueryable<T> Query { get; }

    /// <summary>
    /// Creates a new entity of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="e">The entity to create.</param>
    /// <param name="saveChanges">Whether to save changes immediately after creating the entity.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task Create(T e, bool saveChanges = true);

    /// <summary>
    /// Updates an existing entity of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="e">The entity to update.</param>
    /// <param name="saveChanges">Whether to save changes immediately after updating the entity.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task Update(T e, bool saveChanges = true);

    /// <summary>
    /// Finds an entity of type <typeparamref name="T"/> by its primary key values.
    /// </summary>
    /// <param name="pkVals">The primary key values of the entity to find.</param>
    /// <returns>A <see cref="Task{T}"/> representing the asynchronous operation, with the entity if found, or <c>null</c> if not found.</returns>
    Task<T?> FindByPrimaryKey(params object[] pkVals);

    /// <summary>
    /// Finds all entities of type <typeparamref name="T"/>.
    /// </summary>
    /// <returns>An <see cref="IAsyncEnumerable{T}"/> containing all entities of type <typeparamref name="T"/>.</returns>
    IAsyncEnumerable<T> FindAll();

    /// <summary>
    /// Deletes an existing entity of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="e">The entity to delete.</param>
    /// <param name="saveChanges">Whether to save changes immediately after deleting the entity.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task Delete(T e, bool trySoft = true, bool saveChanges = true);

    /// <summary>
    /// It saves all changes made in the context to the database.
    /// </summary>
    Task SaveChanges();
}

