using SFF.Infra.Repository.Base;

namespace SFF.Infra.Repository.Entities.BasicInformations
{
    public class SpendingType: IEntityBase
    {

        public SpendingType()
        {
        }

        public virtual long id { get; set; }
        public virtual string descricao { get; set; }
        public virtual string desativado { get; set; }

        public virtual DateTimeOffset createdTime { get; set; }
        public virtual DateTimeOffset? updatedTime { get; set; }

        #region RELATIONAL

        #endregion
    }
}
