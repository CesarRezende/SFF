namespace SFF.Infra.Repository.Base
{

    public interface IEntityBase
    {
        DateTime DataInclusao { get; set; }
        DateTime? DataAlteracao { get; set; }
    }
}
