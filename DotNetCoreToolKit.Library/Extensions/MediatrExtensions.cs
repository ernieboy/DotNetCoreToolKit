using DotNetCoreToolKit.Library.Models.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreToolKit.Library.Extensions
{
    public static class MediatrExtensions
    {
        public static async Task DispatchDomainEventsAsync<TContext, TAggregateRoot>(this IMediator mediator, TContext ctx)
            where TContext : DbContext, new()
            where TAggregateRoot : AggregateRoot
        {
            var domainEntities = ctx.ChangeTracker.Entries<TAggregateRoot>().Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());
            var domainEvents = domainEntities.SelectMany(x => x.Entity.DomainEvents).ToList();
            domainEntities.ToList().ForEach(entity => entity.Entity.DomainEvents.Clear());

            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await mediator.Publish(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}
