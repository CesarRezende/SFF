namespace SFF.Domain.SharedKernel.Base
{
    public abstract class AggregateRoot<T>: Entity<T>, IAggregateRoot
    {
        protected AggregateRoot(T id, DateTimeOffset createdTime, DateTimeOffset? updatedTime)
            :base(id)
        {
            CreatedTime = createdTime;
            UpdatedTime = updatedTime;
        }

        public DateTimeOffset CreatedTime { get; protected set; }
        public DateTimeOffset? UpdatedTime { get; protected set; }
    }
}
