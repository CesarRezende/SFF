using SFF.Infra.Repository.Base;

namespace SFF.Infra.Repository.Entities.BasicInformations
{
    public class Family : IEntityBase
    {

        public Family()
        {
        }

        public virtual long id { get; set; }
        public virtual string descricao { get; set; }
        public virtual bool desativado { get; set; }

        public virtual DateTime hora_criacao { get; set; }
        public virtual DateTime? hora_atualizacao { get; set; }

        #region RELATIONAL

        #endregion
    }
}
