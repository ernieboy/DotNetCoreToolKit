using System;
using static DotNetCoreToolKit.Library.Models.Persistence.Enums;

namespace DotNetCoreToolKit.Library.Models.Persistence
{
    public abstract class BaseObjectWithState
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public ObjectState ObjectState { get; set; }

        public byte[] RowVersion { get; set; }

        public bool? Deleted { get; set; }
    }
}
