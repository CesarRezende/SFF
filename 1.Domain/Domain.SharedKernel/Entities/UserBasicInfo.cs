namespace SFF.Domain.SharedKernel.Entities
{
    public class UserBasicInfo : Entity<long>
    {
        public string Login { get; private set; }
        public string Name { get; private set; }
        public bool Administrator { get; private set; }

        public UserBasicInfo(long id) : base(id)
        {

        }

        public UserBasicInfo(long id, string login, string name, bool administrator) : base(id)
        {
            Login = login;
            Name = name;
            Administrator = administrator;
        }
    }
}
