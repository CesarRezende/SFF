using SFF.Domain.Administration.Application.Queriables;
using SFF.Domain.SharedKernel.Base;
using SFF.Infra.Core.CQRS.Interfaces;

namespace SFF.Domain.Administration.Application
{

    public interface IAdministrationAppService : IAppService
    {
        /// <summary>
        /// used to return DTOS without pass through domain
        /// </summary>
        IAdministrationQueryable Query { get; }


        #region User
        //Task<Result> InsertUserAsync(
        //    string fullName,
        //    string phoneNumber,
        //    int retailerId,
        //    int routingNumber,
        //    long bankAccountNumber,
        //    string code,
        //    string encryptedCode
        //    );

        //Task<Result> UpdateUserPhoneNumberAsync(
        //    Guid userId,
        //    string newPhoneNumber,
        //    string code,
        //    string encryptedCode
        //    );

        //Task<Result<User>> GetUserByIdAsync(Guid id);



        #endregion User

        #region Auth
        Task<CommandResult> GeneratePasswordAsync(string plainPassword);
        Task<CommandResult> Authenticate(string login, string password);
        //Task<CommandResult> RefreshToken(string token, string refreshToken);

        #endregion


    }
}
