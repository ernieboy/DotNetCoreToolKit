using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DotNetCoreToolKit.Library.Models.Persistence;
using LinqKit;
using static DotNetCoreToolKit.Library.Models.Persistence.Enums;

namespace DotNetCoreToolKit.Library.Abstractions
{
    public interface IDataRepository<T>
        where T : BaseObjectWithState, IObjectWithState, new()
    {

        Task<int> SaveChanges();

        /// <summary>
        /// Finds an entity by its id.
        /// </summary>
        /// <param name="id">The ID of the entity to search for.</param>
        /// <returns>The entity if found, it's up to the implementer what to return if the entity was not found. They can throw a NotFoundException if needed.</returns>
        Task<T> FindEntityById(long id);

        /// <summary>
        /// Finds an entity by its GUID.
        /// </summary>
        /// <param name="guid">The GUID of the entity to search for.</param>
        /// <returns>The entity if found, it's up to the implementer what to return if the entity was not found. They can throw a NotFoundException if needed.</returns>
        Task<T> FindEntityByGuid(Guid guid);

        /// <summary>
        /// Checks if an entity exists in the repository by its ID.
        /// </summary>
        /// <param name="entityId">The ID of the entity to search for.</param>
        /// <returns>True if the entity was found, false otherwise.</returns>
        Task<bool> EntityExistsById(int entityId);

        /// <summary>
        /// Checks if an entity exists in the repository by its GUID.
        /// </summary>
        /// <param name="entityGuid">The GUID of the entity to search for.</param>
        /// <returns>True if the entity was found, false otherwise.</returns>
        Task<bool> EntityExistsByGuid(Guid entityGuid); 

        /// <summary>
        /// Returns all entities of type T from the repository based on predicate. 
        /// </summary>
        /// <returns>A collection of T entities if found.</returns>
        Task<IEnumerable<T>> FindAllEntitiesByPredicate(Expression<Func<T, bool>> predicate);

        /// <summary>
        ///  Returns a paged collection of T entity. The client can also specify search and paging requirements criteria.
        /// </summary>
        /// <param name="pageNumber">The page number to return  when paging.</param>
        /// <param name="pageSize">The page size when paging.</param>
        /// <param name="sortColumns">The sort columns in the underlying data store. Should be indexed or performance could be severly reduced.</param>
        /// <param name="sortDirections">Either ASC or DESC.</param>
        /// <param name="searchFilter">A predicate to search by</param>
        /// <returns>A collection of type T entities.</returns>
        Task<(IEnumerable<T> records, long totalNumberOfRecords)> FindAllEntitiesByCriteria(
            int? pageNumber,
            int? pageSize,
            string[] sortColumns,
            string[] sortDirections,
            ExpressionStarter<T> searchFilter);

        void AddOrUpdateEntity(T entity);
    }
}
