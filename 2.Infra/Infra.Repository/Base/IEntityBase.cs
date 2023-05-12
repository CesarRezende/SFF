namespace SFF.Infra.Repository.Base
{

    public interface IEntityBase
    {
        DateTime hora_criacao { get; set; }
        DateTime? hora_atualizacao { get; set; }
    }
}
