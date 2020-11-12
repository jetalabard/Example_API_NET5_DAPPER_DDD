namespace Common.Domain.BuildingBlocks
{
    public abstract class Entity<TId>
    {
        public TId Id { get; set; }
    }
}
