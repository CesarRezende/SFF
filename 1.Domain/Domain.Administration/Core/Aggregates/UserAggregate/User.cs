using Flunt.Notifications;
using Flunt.Validations;
using SFF.Domain.Administration.Application.Events;
using SFF.Domain.SharedKernel.Base;
using SFF.Infra.Core.CQRS.Models;
using SFF.Infra.Core.Security.Models;

namespace SFF.Domain.Administration.Core.Aggregates.UserAggregate
{

    public class User : AggregateRoot<long>
    {
        public string Login { get; private set; }
        public string Name { get; private set; }
        public Password Password { get; private set; }
        public bool Administrator { get; private set; }
        public bool Inactived { get; private set; }
        public int? LoginFailTimes { get; private set; }
        public DateTime? BlockedUntil { get; private set; }

        private static int LOGIN_FAIL_TIMES_LIMIT { get; set; } = 5;
        private static int BLOCKED_UNIT_TIME_IN_MINUTES { get; set; } = 10;

        public User(long id,
            string name,
            string login,
            Password password,
            bool administrator,
            bool inactived,
            int? loginFailTimes,
            DateTime? blockedUntil,
            DateTime createdTime,
            DateTime? updatedTime
            ) : base(id, createdTime, updatedTime)
        {
            Login = login;
            Name = name;
            Password = password;
            Administrator = administrator;
            LoginFailTimes = loginFailTimes;
            BlockedUntil = blockedUntil;
            Inactived = inactived;
        }

        public static User CreateUser(
            string name,
            string login,
            string plainPassword,
            bool administrator
            )

        {

            var password = Password.CreatePassword(plainPassword);

            var newUser = new User(default(long),
                login: login,
                name: name,
                password: password,
                administrator: administrator,
                inactived: false,
                loginFailTimes: 0,
                blockedUntil: null,
                createdTime: DateTime.UtcNow,
                updatedTime: DateTime.UtcNow
                );

            newUser.AddNotifications(new Contract<Notification>()
               .Requires()
               .IsNotNullOrEmpty(newUser.Login, "User.Login", "Login não deve ser vazio")
               .IsGreaterOrEqualsThan(newUser.Login.Length, 3, "User.Login", "Login deve ter ao menos 3 caracteres")
               .IsNotNullOrEmpty(newUser.Name, "User.Name", "Nome não deve ser vazio")
               .IsGreaterOrEqualsThan(newUser.Name.Length, 3, "User.Name", "Nome deve ter ao menos 3 caracteres")
               .IsNotNullOrEmpty(plainPassword, "User.Password", "Senha não deve ser vazia")
               .IsGreaterOrEqualsThan(plainPassword, 6, "User.Password", "Nome deve ter ao menos 6 caracteres"));

            newUser.AddNotifications(password.Notifications);

            return newUser;
        }

        public Result AutenticateUser(string plainPassword)
        {

            var result = new Result();
            var bloquedErrorMessage = $"Usuario esta bloqueado porque excedeu o limit de tentativas de login de {LOGIN_FAIL_TIMES_LIMIT} vezes.\r\n Aguarde {BLOCKED_UNIT_TIME_IN_MINUTES} minutos e depois tente novamente";

            if (BlockedUntil.HasValue && BlockedUntil > DateTime.Now)
                result.AddNotification("", bloquedErrorMessage);

            if (!result.IsValid)
                return result;

            var isPasswordValid = Password.ValidatePassword(plainPassword);

            if (isPasswordValid)
            {
                LoginFailTimes = 0;
                BlockedUntil = null;
            }
            else
            {
                LoginFailTimes += 1;

                if (LoginFailTimes >= LOGIN_FAIL_TIMES_LIMIT)
                {
                    BlockedUntil = DateTime.Now.AddMinutes(BLOCKED_UNIT_TIME_IN_MINUTES);
                    result.AddNotification("", bloquedErrorMessage);
                }

                var leftAttempts = LOGIN_FAIL_TIMES_LIMIT - LoginFailTimes ?? 0;
                result.AddNotification("", $"Senha é invalida, você tem mais {leftAttempts} tentativas!");

            }

            return result;
        }

        public Result CreateSession(string ip, long userId, AuthInformation authInformation)
        {

            var result = new Result();


            AddEvent(new UserAuthenticatedEvent(ip, userId, authInformation));

            return result;
        }
    }
}
