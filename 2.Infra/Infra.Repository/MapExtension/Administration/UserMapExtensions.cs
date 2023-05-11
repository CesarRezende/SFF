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
                nome = user.Name,
                administrator = user.Administrator,
                desativado = user.Inactived,
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
                administrator: user.administrator,
                inactived: user.desativado,
                createdTime: user.createdTime,
                updatedTime: user.updatedTime
                );

            return domainEntity;

        }

    }
}
