using Flunt.Notifications;
using Flunt.Validations;
using SFF.Domain.SharedKernel;
using SFF.Infra.Core.Security.Models;

namespace SFF.Domain.Administration.Core.Aggregates.UserAggregate
{
    public class Password : ValueObject<Password>
    {

        public Password()
        {

        }

        public Password(string passwordHash)
        {
            this.PasswordHash = passwordHash;
        }

        private static readonly Guid _passwordSalt = new Guid("562224f7-1416-4a89-bb4e-8f1b4a2c05e5");
        public string PasswordHash { get; private set; }

        protected override bool EqualsCore(Password other)
        {
            return this.PasswordHash == other.PasswordHash;
        }

        protected override int GetHashCodeCore()
        {
            return PasswordHash.GetHashCode();
        }


        public static Password CreatePassword(string plainPassword)

        {
            Password newPassword = new Password();

            newPassword.AddNotifications(new Contract<Notification>()
               .Requires()
               .IsNotNullOrEmpty(plainPassword, "Password.PlainPassword", "A senha é obrigatória"));

            newPassword.PasswordHash = GeneratePasswordHash(plainPassword);

            return newPassword;
        }

        public bool ValidatePassword(string plainPassword)
        {
            var passwordHash = GeneratePasswordHash(plainPassword);

            return this.PasswordHash == passwordHash;
        }


        private static string GeneratePasswordHash(string plainPassword)
        {
            return $"{plainPassword}{_passwordSalt}".ToSha256();
        }

    }
}
