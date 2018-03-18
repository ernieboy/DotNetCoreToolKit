using DotNetCoreToolKit.Library.Models.Persistence;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DotNetCoreToolKit.Library.Abstractions
{
    public interface IDataRepository<T>
        where T : AggregateRoot, IAuditable, IObjectWithState, new()
    {

        Task SaveEntity(T entity);

        /// <summary>
        /// Finds an entity by its id.
        /// </summary>
        /// <param name="id">The ID of the entity to search for.</param>
        /// <returns>The entity if found, it's up to the implementer what to do if the entity was not found.</returns>
        Task<T> FindById(Guid id);

       
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
    }
}
