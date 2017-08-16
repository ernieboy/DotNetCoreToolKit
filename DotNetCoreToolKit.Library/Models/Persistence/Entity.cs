using System;
using System.Collections.Generic;
using DotNetCoreToolKit.Library.Abstractions;
using static DotNetCoreToolKit.Library.Models.Persistence.Enums;

namespace DotNetCoreToolKit.Library.Models.Persistence
{
    public abstract class Entity: IAggregateRoot
	{

		int? _requestedHashCode;
		int _Id;

		private List<IDomainEvent> _domainEvents;

		public virtual int Id
		{
			get
			{
				return _Id;
			}
			protected set
			{
				_Id = value;
			}
		}

		public Guid Guid { get; private set; }

		public ObjectState ObjectState { get; set; }

		public byte[] RowVersion { get; set; }

		public bool? Deleted { get; set; }

		public List<IDomainEvent> DomainEvents => _domainEvents;
        public void AddDomainEvent(IDomainEvent domainEvent)
		{
			_domainEvents = _domainEvents ?? new List<IDomainEvent>();
			_domainEvents.Add(domainEvent);
		}

        public void RemoveDomainEvent(IDomainEvent domainEvent)
		{
			if (_domainEvents is null) return;
			_domainEvents.Remove(domainEvent);
		}

		public bool IsTransient()
		{
			return this.Id == default(Int32);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is Entity))
				return false;

			if (Object.ReferenceEquals(this, obj))
				return true;

			if (this.GetType() != obj.GetType())
				return false;

			Entity item = (Entity)obj;

			if (item.IsTransient() || this.IsTransient())
				return false;
			else
				return item.Id == this.Id;
		}

		public override int GetHashCode()
		{
			if (!IsTransient())
			{
				if (!_requestedHashCode.HasValue)
					_requestedHashCode = this.Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

				return _requestedHashCode.Value;
			}
			else
				return base.GetHashCode();

		}

		public static bool operator ==(Entity left, Entity right)
		{
			if (Object.Equals(left, null))
				return (Object.Equals(right, null)) ? true : false;
			else
				return left.Equals(right);
		}

		public static bool operator !=(Entity left, Entity right)
		{
			return !(left == right);
		}
	}
}
