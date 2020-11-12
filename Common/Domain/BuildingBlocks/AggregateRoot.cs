using System;

namespace Common.Domain.BuildingBlocks
{
    public abstract class AggregateRoot<TId>
    {
        public TId Id { get; set; }

        protected AggregateRoot()
        {
            Id = (TId)Activator.CreateInstance(typeof(TId), new object[] { Guid.NewGuid() });
        }
    }
}
