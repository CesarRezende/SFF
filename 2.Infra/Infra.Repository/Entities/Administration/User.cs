using SFF.Infra.Repository.Base;

namespace SFF.Infra.Repository.Entities.Administration
{
    public class User : IEntityBase
    {
        public User()
        {
        }

        public virtual long id { get; set; }
        public virtual string login { get; set; }
        public virtual string nome { get; set; }
        public virtual string senha { get; set; }
        public virtual bool administrator { get; set; }
        public virtual bool desativado { get; set; }
        public virtual int? numero_falhas_login { get; set; }
        public virtual DateTime? bloqueado_ate { get; set; }


        public virtual DateTime hora_criacao { get; set; }
        public virtual DateTime? hora_atualizacao { get; set; }

        #region RELATIONAL


        #endregion
    }
}
