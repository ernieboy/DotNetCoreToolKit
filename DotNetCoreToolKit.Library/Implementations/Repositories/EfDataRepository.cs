using DotNetCoreToolKit.Library.Abstractions;
using DotNetCoreToolKit.Library.Extensions;
using DotNetCoreToolKit.Library.Models.Persistence;
using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DotNetCoreToolKit.Library.Extensions;

namespace DotNetCoreToolKit.Library.Implementations.Repositories
{
    public abstract class EfDataRepository<T, TContext> : IDataRepository<T>
        where T : AggregateRoot, new()
        where TContext : DbContext, new()
    {

        /// <summary>
        /// DbContext must be passed in by the extending class.
        /// </summary>
        protected TContext context;
        private readonly IMediator _mediatr;

        public EfDataRepository()
        {
        }

        protected EfDataRepository(TContext context, IMediator mediatr)
        {
            this.context = context;
            _mediatr = mediatr;
        }

        public virtual async Task<(IEnumerable<T> records, long totalNumberOfRecords)> FindAllEntitiesByCriteria(
            int? pageNumber, int? pageSize, string[] sortColumns, string[] sortDirections, ExpressionStarter<T> searchFilter)
        {
            return await FindAllByCriteria(
                pageNumber, pageSize,
                 sortColumns, sortDirections, searchFilter);
        }


        private async Task<(IEnumerable<T> records, long totalNumberOfRecords)> FindAllByCriteria(
            int? pageNumber,
            int? pageSize,
            string[] sortColumns,
            string[] sortDirections,
            ExpressionStarter<T> searchPredicate)
        {
            int pageIndex = pageNumber ?? 1;
            int sizeOfPage = pageSize ?? 10;
            if (pageIndex < 1) pageIndex = 1;
            if (sizeOfPage < 1) sizeOfPage = 5;
            int skipValue = (sizeOfPage * (pageIndex - 1));
            var searchFilter = searchPredicate ?? BuildDefaultSearchFilterPredicate();

            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            long totalRecords = context.Set<T>().AsExpandable().Where(searchFilter).Count();

            var list = context.Set<T>().AsExpandable()
                    .Where(searchFilter);

            if (sortColumns != null && sortDirections != null)
            {
                if (sortColumns.Any() && (sortColumns.Length == sortDirections.Length))
                {
                    var orderBy = new StringBuilder($"{sortColumns[0]} {sortDirections[0]}");
                    for (int count = 1; count < sortColumns.Length; count++)
                    {
                        orderBy.Append($", {sortColumns[count]} {sortDirections[count]}");
                    }
                    list = list.OrderBy($"{orderBy.ToString()}");
                }
            }

            list = list.Skip(skipValue)
                    .Take(sizeOfPage);

            return (await list.ToListAsync(), totalRecords);
        }

        /// <summary>
        /// Default predicate for when the client did not provide a predicate for searching. 
        /// In that case use this predicated since the search predicate is always required.
        /// This predicate just returns TRUE, which means NO filtering.
        /// </summary>
        /// <returns>An expression to use for filtering</returns>
        protected virtual ExpressionStarter<T> BuildDefaultSearchFilterPredicate()
        {
            Expression<Func<T, bool>> filterExpression = a => true;
            ExpressionStarter<T> predicate = PredicateBuilder.New(filterExpression);
            return predicate;
        }

        public virtual async Task<IEnumerable<T>> FindAllEntitiesByPredicate(Expression<Func<T, bool>> predicate)
        {
            var items = await context.Set<T>().Where(predicate).ToListAsync();
            return items;
        }

        public async Task<T> FindById(Guid guid)    
        {
            var entity = await context.Set<T>().FirstOrDefaultAsync(e => e.Id == guid);
            return entity;
        }

        public virtual async Task SaveEntity(T entity)
        {
            context.ApplyStateChanges();
            AuditEntities();
            await _mediatr.DispatchDomainEventsAsync<TContext,T>(context);
            int affected = await context.SaveChangesAsync();
        }

        private void AuditEntities()
        {
            // Get the authenticated user name 
            string userName = string.Empty;

            var user = ClaimsPrincipal.Current;
            if (user != null)
            {
                var identity = user.Identity;
                if (identity != null)
                {
                    userName = identity.Name;
                }
            }

            // Get current date & time
            DateTime now = DateTime.Now;

            // For every changed entity marked as IAuditable set the values for the audit properties
            foreach (EntityEntry<IAuditable> entry in context.ChangeTracker.Entries<IAuditable>())
            {
                // If the entity was added.
                if (entry.State == EntityState.Added)
                {
                    entry.Property("CreatedBy").CurrentValue = userName;
                    entry.Property("CreatedAt").CurrentValue = now;
                }
                else if (entry.State == EntityState.Modified) // If the entity was updated
                {
                    entry.Property("UpdatedBy").CurrentValue = userName;
                    entry.Property("UpdatedAt").CurrentValue = now;
                }
            }
        }
    }
}
