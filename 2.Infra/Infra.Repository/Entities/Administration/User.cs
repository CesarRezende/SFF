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
        public virtual bool administrator { get; set; }
        public virtual bool desativado { get; set; }


        public virtual DateTimeOffset CreatedTime { get; set; }
        public virtual DateTimeOffset? UpdatedTime { get; set; }

        #region RELATIONAL


        #endregion
    }
}
