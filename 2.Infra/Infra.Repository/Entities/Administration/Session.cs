using SFF.Infra.Repository.Base;

namespace SFF.Infra.Repository.Entities.Administration
{
    public class Session : IEntityBase
    {
        public virtual long id { get; set; }
        public virtual string ip { get; set; }
        public virtual DateTime data_expiracao { get; set; }
        public virtual string refresh_token { get; set; }
        public virtual DateTime data_expiracao_refresh_token { get; set; }
        public virtual DateTime hora_criacao { get; set; }
        public virtual DateTime? hora_atualizacao { get; set; }


        public virtual long usuario_id { get; set; }
        public virtual User User { get; set; }


    }
}
