using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SFF.Domain.Administration.Application.Queriables;
using SFF.Domain.Administration.Core.Aggregates.UserAggregate;
using SFF.Infra.Core.CQRS.Implementation;
using SFF.Infra.Core.CQRS.Interfaces;
using SFF.SharedKernel.Helpers;

namespace SFF.Domain.Administration.Application.AppServices
{
    public class AdministrationAppService : BaseAppService, IAdministrationAppService
    {

        private readonly IEventDispatcher _dispatcher;
        //private readonly IUserRepository _userRepository;
        private IConfiguration _configuration;
        private readonly ILogger<AdministrationAppService> _logger;

        public IAdministrationQueryable Query { get; }

        public AdministrationAppService(
            IAdministrationQueryable queryable,
            //IUserRepository userRepository,
            IEventDispatcher dispatcher,
            ILogger<AdministrationAppService> logger,
            IConfiguration configuration)
        {

            Query = queryable != null ? queryable : throw new ArgumentNullException("queryable");
            //_userRepository = userRepository != null ? userRepository : throw new ArgumentNullException("userRepository");
            _dispatcher = dispatcher != null ? dispatcher : throw new ArgumentNullException("dispatcher");
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


        //public async Task<Result<UserAuthInformation>> GetUserInformationForAuth(string phoneNumber)
        //{
        //    var userAuthInformationDTO = new Result<UserAuthInformation>();

        //    userAuthInformationDTO.Data = await Query.GetUserAuthInformation(phoneNumber);

        //    if (userAuthInformationDTO.Data is null)
        //    {
        //        _logger.LogWarning($"User not found");
        //        return new Result<UserAuthInformation>().AddError(MessagesHelper.GetMessage("UserNotFoundMSg")).SetAsNotFound();
        //    }

        //    var userLanguage = await Query.GetUserLanguage(userAuthInformationDTO.Data.Id);
        //    userAuthInformationDTO.Data.Language = userLanguage == null ? "en-US" : userLanguage;

        //    var retailerInfo = await _kornerstoneRetailerApi.GetRetailerInformation(userAuthInformationDTO.Data.DefaultRetailer.Id);

        //    if (retailerInfo.IsValid && !retailerInfo.IsNotFound)
        //    {
        //        userAuthInformationDTO.Data.DefaultRetailer.Name = retailerInfo?.Data?.Name;
        //        userAuthInformationDTO.Data.DefaultRetailer.StateId = retailerInfo.Data?.Address?.StateId;

        //        var retailerGroup = await _kornerstoneRetailerApi.GetRetailerGroupInformation(userAuthInformationDTO.Data.DefaultRetailer.Id);

        //        if (retailerGroup.Succeded && retailerGroup.Data.Count() != 0)
        //            foreach (var retailer in userAuthInformationDTO.Data.Retailers)
        //            {
        //                retailer.Name = retailerGroup.Data.FirstOrDefault(x => x.Id == retailer.Id)?.Name;
        //            }


        //        userAuthInformationDTO.Data.RaffleConfig = await Query.GetRaffleConfigInfo(retailerInfo.Data.Address.StateId);

        //        return userAuthInformationDTO;
        //    }
        //    else
        //    {
        //        _logger.LogWarning($"An unexpected error occurred while trying to get user information for auth");
        //        return new Result<UserAuthInformation>().SetAsInternalServerError().AddErrors(retailerInfo.Notifications.ToList());
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


        //public async Task<Result> SaveAuthTokenAsync(string securityStamp, Guid userId, AppLanguage language, string expoToken)
        //{
        //    var result = new Result();

        //    try
        //    {
        //        var authToken = await _tokenAuthRepository.GetByIdAsync(userId);

        //        if (authToken is null)
        //        {
        //            var newTokenAuth = TokenAuth.CreateTokenAuth(securityStamp: securityStamp, userId: userId, MessagesHelper.GetLanguageAcronym(language), expoToken);

        //            _logger.LogDebug($"TokenAuth: {newTokenAuth.ToJsonFormat()}");

        //            if (newTokenAuth.IsValid)
        //            {
        //                await _tokenAuthRepository.InsertAsync(newTokenAuth);
        //                _logger.LogInformation($"Dispatching  tokenAuth {newTokenAuth.Id} domain events");
        //                await _dispatcher.DispatchAll(newTokenAuth.DomainEvents);

        //                _logger.LogInformation($"TokenAuth {newTokenAuth.Id} inserted successfully!");
        //            }
        //            else
        //            {
        //                _logger.LogWarning($"TokenAuth {newTokenAuth.Id} is invalid!");
        //                _logger.LogWarning(newTokenAuth.Notifications.CreateLogMsg());
        //            }

        //            return new Result(newTokenAuth.Notifications);
        //        }

        //        authToken.GenerateUpdateTokenAuth(securityStamp, MessagesHelper.GetLanguageAcronym(language), expoToken);

        //        if (authToken.IsValid)
        //        {
        //            await _tokenAuthRepository.UpdateAsync(authToken);
        //            await _dispatcher.DispatchAll(authToken.DomainEvents);
        //        }

        //        return new Result(authToken.Notifications);

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"An unexpected error occurred while trying to insert or update the token of user with id { userId }");
        //        _logger.LogError(ex.ToString());

        //        return new Result().SetAsInternalServerError().AddError(MessagesHelper.GetMessage("AnUnexpectedErrorInsertTokenErrorMsg", userId.ToString()));
        //    }
        //}

        //public async Task<Result<Auth>> Authenticate(string login, string password, string expoToken)
        //{
        //    try
        //    {
        //        var isValidationCodeValidOP = await ValidateCredentialsAsync(login, password);

        //        if (!isValidationCodeValidOP.IsValid)
        //            return new Result<Auth>().AddErrors(isValidationCodeValidOP.Notifications.ToList());

        //        var isValidationCodeValid = isValidationCodeValidOP.Data;
        //        if (isValidationCodeValid)
        //        {

        //            var phoneSuccssedparsed = login.TryParseToPhoneNumber(out var phone);

        //            if (!phoneSuccssedparsed)
        //            {
        //                _logger.LogWarning($"PhoneNumber is invalid");
        //                return new Result<Auth>().AddError(MessagesHelper.GetMessage("PhoneNumberNotValidErrorMsg"));
        //            }

        //            var userAuthInformation = await GetUserInformationForAuth(phone.ToInternationalFormat());

        //            if (!userAuthInformation.Succeded)
        //            {
        //                if (userAuthInformation.IsNotFound)
        //                {
        //                    return new Result<Auth>().SetAsNotFound().AddError(MessagesHelper.GetMessage("NotFoundUserInfoByPhoneErrorMsg", login));
        //                }
        //                else
        //                {
        //                    return new Result<Auth>().AddError(MessagesHelper.GetMessage("AnUnexpectedErrorLoginErrorMsg"));
        //                }
        //            }

        //            var refreshToken = _tokenService.GenerateRefreshToken();
        //            var token = _tokenService.GenerateToken(refreshToken, language: userAuthInformation.Data.Language, user: userAuthInformation.Data);
        //            var retailerID = userAuthInformation.Data.DefaultRetailer.Id.ToString();
        //            var fullName = userAuthInformation.Data.FullName;

        //            var authData = new Auth(
        //                token: token,
        //                refreshToken: refreshToken,
        //                retailerID: retailerID,
        //                fullName: fullName
        //                );

        //            var saveToken = await SaveAuthTokenAsync(authData.RefreshToken, userAuthInformation.Data.Id, MessagesHelper.GetAppLanguage(userAuthInformation.Data.Language), expoToken);

        //            if (!saveToken.IsValid)
        //            {
        //                return new Result<Auth>().AddErrors(saveToken.Notifications.Select(x => x.Message));
        //            }

        //            return new Result<Auth>(authData);
        //        }
        //        else
        //        {
        //            _logger.LogWarning($"Validation code is invalid or is expired");
        //            return new Result<Auth>().AddError(MessagesHelper.GetMessage("CodeInvalidOrExpiredErrorMsg"));

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An unexpected error occurred during the login");
        //        return new Result<Auth>().SetAsInternalServerError().AddError(MessagesHelper.GetMessage("AnUnexpectedErrorLoginErrorMsg"));
        //    }
        //}

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
