using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SFF.Domain.Administration.Application.Queriables;
using SFF.Domain.Administration.Application.Queriables.QueryResult;
using SFF.Infra.Repository.Base;

namespace SFF.Infra.Repository.Repositories.Administration
{
    public class AdministrationRepository : BaseRepository, IAdministrationQueryable
    {

        public AdministrationRepository(SFFDbContext dbContext, ILogger<AdministrationRepository> logger)
            : base(dbContext, logger)
        {

        }

        public async Task<IEnumerable<UserQueryResult>> GetAll()
        {
            try
            {
                var userAuthInfo = await _dbContext.User
                    .Select(x => new UserQueryResult
                    {
                        Id = x.id,
                        Login = x.login,
                        Name = x.nome,
                        Administrator = x.administrator,
                        Inactivated = x.desativado,

                    }).AsNoTracking().ToListAsync();

                return userAuthInfo;

            }
            catch (Exception ex)
            {
                var msgErro = $"An unexpected error occurred while trying to get users";
                _logger.LogError(ex, msgErro);
                throw;
            }
        }

        //public async Task<UserAuthInformation> GetUserAuthInformation(string phoneNumber)
        //{
        //    try
        //    {
        //        var userAuthInfo = await _dbContext.User.Where(x => x.PhoneNumber == phoneNumber)
        //            .Select(x => new UserAuthInformation
        //            {
        //                Id = x.Id,
        //                FullName = x.FullName,
        //                PhoneNumber = x.PhoneNumber,
        //                CreatedTime = x.CreatedTime,
        //                Retailers = x.UserRetailers.Select(x => new UserAuthInformation.Retailer()
        //                {
        //                    Id = x.RetailerId,
        //                    IsDefaultRetailer = x.IsDefaultRetailer,
        //                }).ToList(),
        //                Active = x.Active

        //            }).AsNoTracking().FirstOrDefaultAsync();

        //        return userAuthInfo;

        //    }
        //    catch (Exception ex)
        //    {
        //        var msgErro = $"An unexpected error occurred while trying to get user information by phone number : { phoneNumber }";
        //        _logger.LogError(ex, msgErro);
        //        throw;
        //    }
        //}


    }
}
