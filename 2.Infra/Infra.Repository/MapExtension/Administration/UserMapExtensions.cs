using db = SFF.Infra.Repository.Entities.Administration;
using domain = SFF.Domain.Administration.Core.Aggregates.UserAggregate;

namespace SFF.Infra.Repository.MapExtension.Administration
{
    public static class UserMapExtensions
    {

        public static db.User ToDbEntity(this domain.User user)
        {

            if (user == null)
                throw new ArgumentException("Domain entity can not be null");


            var dbEntity = new db.User()
            {
                id = user.Id,
                login = user.Login,
                senha = user.Password.PasswordHash,
                nome = user.Name,
                administrator = user.Administrator,
                desativado = user.Inactived,
                numero_falhas_login = user.LoginFailTimes,
                bloqueado_ate =  user.BlockedUntil,
                //CreatedTime = user.CreatedTime,
                //UpdatedTime = user.UpdatedTime,
            };

            return dbEntity;
        }

        public static domain.User? ToDomain(this db.User user)
        {
            if (user == null)
                return default;


            var domainEntity = new domain.User(
                id: user.id,
                name: user.nome,
                login: user.login,
                password:new domain.Password(user.senha),
                administrator: user.administrator,
                inactived: user.desativado,
                loginFailTimes: user.numero_falhas_login,
                blockedUntil: user.bloqueado_ate,
                createdTime: user.hora_criacao,
                updatedTime: user.hora_atualizacao
                );

            return domainEntity;

        }

    }
}
