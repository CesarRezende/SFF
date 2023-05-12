namespace SFF.Domain.SharedKernel.Base
{
    public abstract class AggregateRoot<T>: Entity<T>, IAggregateRoot
    {
        protected AggregateRoot(T id, DateTime createdTime, DateTime? updatedTime)
            :base(id)
        {
            CreatedTime = createdTime;
            UpdatedTime = updatedTime;
        }

        public DateTime CreatedTime { get; protected set; }
        public DateTime? UpdatedTime { get; protected set; }
    }
}
