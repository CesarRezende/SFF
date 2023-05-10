namespace SFF.Infra.Repository.Base
{

    public interface IEntityBase
    {
        DateTimeOffset CreatedTime { get; set; }
        DateTimeOffset? UpdatedTime { get; set; }
    }
}
