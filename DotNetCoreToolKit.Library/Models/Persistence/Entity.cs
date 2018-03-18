using DotNetCoreToolKit.Library.Abstractions;
using System;
using static DotNetCoreToolKit.Library.Models.Persistence.Enums;

namespace DotNetCoreToolKit.Library.Models.Persistence
{
    public abstract class Entity: IAuditable, IObjectWithState
    {
		public Guid Id { get; private set; }

		public ObjectState ObjectState { get; set; }

		public byte[] RowVersion { get; set; }

		public bool? Deleted { get; set; }

		public bool IsTransient()
		{
			return Id == default(Guid);
		}

	}
}
