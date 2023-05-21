namespace SFF.Infra.Core.Security.Models
{

    public class UserAuthInformation
    {

        public UserAuthInformation(long id, string login)
        {
            Id = id;
            Login = login;
        }

        public long Id { get; private set; }
        public string Login { get; private set; }

    }
}
