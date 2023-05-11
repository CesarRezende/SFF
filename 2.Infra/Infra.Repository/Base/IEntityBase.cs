namespace SFF.Infra.Repository.Base
{

    public interface IEntityBase
    {
        DateTimeOffset createdTime { get; set; }
        DateTimeOffset? updatedTime { get; set; }
    }
}
