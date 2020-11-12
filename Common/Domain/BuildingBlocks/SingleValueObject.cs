namespace Common.Domain.BuildingBlocks
{
    public abstract record SingleValueObject<T>
    {
        public T Value { get; }

        protected SingleValueObject(T value)
        {
            Value = value;
        }
    }
}