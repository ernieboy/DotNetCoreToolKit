using DotNetCoreToolKit.Library.Abstractions;
using MediatR;
using System.Collections.Generic;

namespace DotNetCoreToolKit.Library.Models.Persistence
{
    public class AggregateRoot : Entity, IAggregateRoot
    {
        private List<INotification> _domainEvents;

        public List<INotification> DomainEvents => _domainEvents;

        public void AddDomainEvent(INotification domainEvent)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(domainEvent);
        }

        public void RemoveDomainEvent(INotification domainEvent)
        {
            if (_domainEvents is null) return;
            _domainEvents.Remove(domainEvent);
        }

    }
}
