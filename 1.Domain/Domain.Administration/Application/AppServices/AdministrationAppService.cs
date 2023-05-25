using EaiBrasil.Kornerstone.KashApp.Infra.Security.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SFF.Domain.Administration.Application.Queriables;
using SFF.Domain.Administration.Core.Aggregates.SessionAggegate;
using SFF.Domain.Administration.Core.Aggregates.UserAggregate;
using SFF.Domain.Administration.Core.Repositories;
using SFF.Infra.Core.CQRS.Implementation;
using SFF.Infra.Core.CQRS.Interfaces;
using SFF.Infra.Core.CQRS.Models;
using SFF.Infra.Core.Security.Models;
using SFF.SharedKernel.Helpers;

namespace SFF.Domain.Administration.Application.AppServices
{
    public class AdministrationAppService : BaseAppService, IAdministrationAppService
    {

        private readonly IUserRepository _userRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly ITokenService _tokenService;
        private IConfiguration _configuration;
        private readonly ILogger<AdministrationAppService> _logger;

        public IAdministrationQueryable Query { get; }

        public AdministrationAppService(
            IAdministrationQueryable queryable,
            IUserRepository userRepository,
            ISessionRepository sessionRepository,
            ITokenService tokenService,
            ILogger<AdministrationAppService> logger,
            IConfiguration configuration)
        {

            Query = queryable != null ? queryable : throw new ArgumentNullException("queryable");
            _userRepository = userRepository != null ? userRepository : throw new ArgumentNullException("userRepository");
            _tokenService = tokenService != null ? tokenService : throw new ArgumentNullException("tokenService");
            _sessionRepository = sessionRepository != null ? sessionRepository : throw new ArgumentNullException("sessionRepository");
            _logger = logger != null ? logger : throw new ArgumentNullException("logger");
            _configuration = configuration != null ? configuration : throw new ArgumentNullException("configuration");
        }


        public void Dispose()
        {
            //_userRepository.Dispose();
        }


        #region AdministrationAppService

        #region User
        //public async Task<Result> InsertUserAsync(
        //    string fullName,
        //    string phoneNumber,
        //    int retailerId,
        //    int routingNumber,
        //    long bankAccountNumber,
        //    string code,
        //    string encryptedCode

        //    )
        //{
        //    try
        //    {
        //        var result = new Result();

        //        var isValidationCodeValidOp = await ValidateValidationCodeAsync(code, encryptedCode);

        //        if (!isValidationCodeValidOp.IsValid)
        //            return new Result().AddErrors(isValidationCodeValidOp.Notifications.ToList());

        //        var isvalidationCodeValid = isValidationCodeValidOp.Data;
        //        if (isvalidationCodeValid)
        //        {

        //            //Validar se retailer id existe no sistema da KornerStone
        //            var retailer = await _kornerstoneRetailerApi.GetRetailerInformation(retailerId);

        //            if (retailer.IsValid)
        //            {
        //                var retailerGroup = await _kornerstoneRetailerApi.GetRetailerGroupInformation(retailerId);

        //                if (retailerGroup.IsValid || retailerGroup.IsNotFound)
        //                {
        //                    var retailersIds = retailerGroup.Data.Select(x => x.Id);

        //                    var newUser = User.CreateUser(
        //                        fullName: fullName,
        //                        phoneNumber: phoneNumber,
        //                        defaultRetailerId: retailerId,
        //                        retailersIds: retailersIds,
        //                        routingNumber: routingNumber,
        //                        bankAccountNumber: bankAccountNumber
        //                        );

        //                    _logger.LogDebug($"User: {newUser.ToJsonFormat()}");


        //                    if (newUser.IsValid)
        //                    {

        //                        if (await _userRepository.Exists(newUser.PhoneNumber.ToInternationalFormat()))
        //                        {
        //                            var errorMsg = $"Already existe a user associate to the phone number {phoneNumber}";
        //                            _logger.LogWarning(errorMsg);

        //                            return result.AddError(MessagesHelper.GetMessage("PhoneAlreadyAssociateToUserErrorMsg", phoneNumber));
        //                        }

        //                        await _userRepository.InsertAsync(newUser);
        //                        _logger.LogInformation($"Dispatching  user {newUser.Id} domain events");
        //                        await _dispatcher.DispatchAll(newUser.DomainEvents);

        //                        _logger.LogInformation($"User {newUser.Id} inserted successfully!");
        //                    }
        //                    else
        //                    {
        //                        _logger.LogWarning($"User {newUser.Id} is invalid!");
        //                        _logger.LogWarning(newUser.Notifications.CreateLogMsg());
        //                    }

        //                    return new Result(newUser.Notifications);

        //                }
        //                else
        //                {
        //                    _logger.LogWarning($"An unexpected error occurred while trying to insert the user");
        //                    return new Result().AddErrors(retailerGroup.Notifications.ToList());
        //                }
        //            }
        //            else if (retailer.IsNotFound)
        //            {
        //                _logger.LogWarning($"Could not find retailer {retailerId}");
        //                return new Result().SetAsNotFound().AddErrors(retailer.Notifications.ToList());
        //            }
        //            else
        //            {
        //                _logger.LogWarning($"An unexpected error occurred while trying to insert the user");
        //                return new Result().AddErrors(retailer.Notifications.ToList());
        //            }
        //        }
        //        else
        //        {
        //            _logger.LogWarning($"Validation code is invalid or expired");
        //            return new Result().AddError(MessagesHelper.GetMessage("CodeInvalidOrExpiredErrorMsg"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"An unexpected error occurred while trying to insert the user");
        //        _logger.LogError(ex.ToString());

        //        return new Result().SetAsInternalServerError().AddError(MessagesHelper.GetMessage("AnUnexpectedErrorInsertUserErrorMsg"));
        //    }
        //}

        #endregion User

        #region Auth

        public async Task<CommandResult> GeneratePasswordAsync(string plainPassword)
        {
            try
            {

                _logger.LogInformation($"Generating new password Hash");
                var newPassword = Password.CreatePassword(plainPassword);

                if (!newPassword.IsValid)
                    return Result.Failed($"Ocorreu um erro inesperado ao tentar criar uma nova senha. {newPassword.Notifications.CreateLogMsg()}");

                return Result.Success(newPassword.PasswordHash);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred while trying to generate a new password", ex);
                return Result.Failed($"Ocorreu um erro inesperado ao tentar criar uma nova senha");
            }
        }



        public async Task<CommandResult> Authenticate(string ip, string login, string password)
        {
            try
            {
                _logger.LogInformation($"Looking for user {login}");
                var user = await _userRepository.GetByLoginAsync(login);

                if (user == null) { 
                    _logger.LogError($"User {login} not found!");
                    return Result.Failed($"Usuario {login} não foi encontrado!");
                }


                _logger.LogInformation($"Autenticando usuario {login}");
                var isValidCredentials = user.AutenticateUser(password);


                _logger.LogInformation($"Atualiza usuario {login}");
                await _userRepository.UpdateAsync(user);

                if (!isValidCredentials.IsValid) {
                    _logger.LogInformation($"Autenticação do usuario {login} falhou {isValidCredentials.Notifications.CreateLogMsg()}");
                    return Result.Failed(isValidCredentials.Notifications.CreateLogMsg());
                }


                _logger.LogInformation($"Gerando token JWT para o usuario {login}");
                var authInformation = _tokenService.GenerateJWTToken(user: new UserAuthInformation(
                    id: user.Id,
                    login: user.Login
                    ));


                var newSession = Session.CreateSession(
                    ip: ip,
                    expireTime: DateTime.Now.AddSeconds(authInformation.expires_in),
                    refreshToken: authInformation.refresh_token,
                    refreshTokenExpireTime: DateTime.Now.AddSeconds(authInformation.expires_in)
                    );

                await _sessionRepository.InsertAsync(newSession);

                return Result.Success(authInformation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during the login");
                return Result.Failed("Ocorreu um erro inesperado ao tentar efetuar o login");
            }
        }

        //public async Task<Result<Auth>> RefreshToken(string token, string refreshToken, string expoToken)
        //{
        //    try
        //    {
        //        var principal = _tokenService.GetPrincipalFromExpiredToken(token);
        //        Guid userId = Guid.Parse(principal.FindFirst(ClaimTypes.PrimarySid).Value);

        //        var getSavedRefreshToken = await this.Query.GetUserToken(userId);

        //        if (getSavedRefreshToken.SecurityStamp != refreshToken)
        //        {
        //            _logger.LogError("An error ocurred in Refresh Endpoint: Invalid refresh token.");
        //            return new Result<Auth>().AddError(MessagesHelper.GetMessage("InvalidRefreshTokenErrorMsg"));
        //        }

        //        var newRefreshToken = _tokenService.GenerateRefreshToken();
        //        var newJwtToken = _tokenService.GenerateToken(newRefreshToken, getSavedRefreshToken.Language, claims: principal.Claims.ToList());

        //        var saveToken = await this.SaveAuthTokenAsync(newRefreshToken, userId, MessagesHelper.GetAppLanguage(getSavedRefreshToken.Language), expoToken);

        //        if (!saveToken.IsValid)
        //        {
        //            return new Result<Auth>().AddError(MessagesHelper.GetMessage("AnUnexpectedErrorMsg"));
        //        }

        //        var auth = new Auth(newJwtToken, newRefreshToken);

        //        return new Result<Auth>(auth);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An unexpected error occurred during the renew of refresh token");
        //        return new Result<Auth>().SetAsInternalServerError().AddError(MessagesHelper.GetMessage("AnUnexpectedErrorMsg"));
        //    }
        //}



        #endregion

        #endregion AdministrationAppService
    }
}
