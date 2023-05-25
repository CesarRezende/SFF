namespace SFF.Infra.Core.Security.Models
{

    public class UserAuthInformation
    {

        public UserAuthInformation(long id, string login, string name)
        {
            Id = id;
            Login = login;
            Name = name;
        }

        public long Id { get; private set; }
        public string Login { get; private set; }
        public string Name { get; private set; }

    }
}
