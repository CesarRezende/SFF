using SFF.Domain.Administration.Core.Aggregates.SessionAggegate;
using db = SFF.Infra.Repository.Entities.Administration;
using domain = SFF.Domain.Administration.Core.Aggregates.SessionAggegate;

namespace SFF.Infra.Repository.MapExtension.Administration
{
    public static class SessionMapExtensions
    {

        public static db.Session ToDbEntity(this domain.Session Session)
        {

            if (Session == null)
                throw new ArgumentException("Domain entity can not be null");


            var dbEntity = new db.Session()
            {
                id = Session.Id,
                ip = Session.IPAddress.IP,
                data_expiracao = Session.ExpireTime,
                refresh_token = Session.RefreshToken.Token,
                data_expiracao_refresh_token = Session.RefreshToken.ExpireTime,
                hora_criacao = Session.CreatedTime,
                hora_atualizacao = Session.UpdatedTime,
            };

            return dbEntity;
        }

        public static domain.Session? ToDomain(this db.Session Session)
        {
            if (Session == null)
                return default;

            var domainEntity = new domain.Session(
                id: Session.id,
                ipAddress: new IPAddress(Session.ip),
                expireTime: Session.data_expiracao,
                refreshToken: new RefreshToken(Session.refresh_token, Session.data_expiracao_refresh_token),
                createdTime: Session.hora_criacao,
                updatedTime: Session.hora_atualizacao
                );

            return domainEntity;

        }

    }
}
