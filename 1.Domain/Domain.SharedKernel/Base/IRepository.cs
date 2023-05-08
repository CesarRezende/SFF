namespace SFF.Domain.SharedKernel.Base
{
    public interface IRepository<TEntity> : IDisposable where TEntity : IAggregateRoot
    {
        Task<TEntity> GetByIdAsync(Guid id);
        Task InsertAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}