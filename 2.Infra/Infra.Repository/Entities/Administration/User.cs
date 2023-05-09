namespace SFF.Infra.Repository.Entities.Administration
{
    public class User
    {
        public User()
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }


        public virtual DateTimeOffset CreatedTime { get; set; }
        public virtual DateTimeOffset UpdatedTime { get; set; }

        #region RELATIONAL


        #endregion
    }
}
