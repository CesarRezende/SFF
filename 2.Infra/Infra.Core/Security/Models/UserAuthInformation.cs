namespace SFF.Infra.Core.Security.Models
{

    public class UserAuthInformation
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public bool Active { get; set; }
        public string Language { get; set; }

    }
}
