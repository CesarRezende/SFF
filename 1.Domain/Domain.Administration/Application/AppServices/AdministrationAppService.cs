using EaiBrasil.Kornerstone.KashApp.Infra.Security.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SFF.Domain.Administration.Application.Queriables;
using SFF.Domain.Administration.Core.Aggregates.SessionAggegate;
using SFF.Domain.Administration.Core.Aggregates.UserAggregate;
using SFF.Domain.Administration.Core.Repositories;
using SFF.Infra.Core.CQRS.Implementation;
using SFF.Infra.Core.CQRS.Interfaces;
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

                if (user == null)
                {
                    _logger.LogError($"User {login} not found!");
                    return Result.Failed($"Usuario {login} não foi encontrado!");
                }


                _logger.LogInformation($"Autenticando usuario {login}");
                var isValidCredentials = user.AutenticateUser(password);


                _logger.LogInformation($"Atualiza usuario {login}");
                await _userRepository.UpdateAsync(user);

                if (!isValidCredentials.IsValid)
                {
                    _logger.LogInformation($"Autenticação do usuario {login} falhou {isValidCredentials.Notifications.CreateLogMsg()}");
                    return Result.Failed(isValidCredentials.Notifications.CreateLogMsg());
                }


                _logger.LogInformation($"Gerando token JWT para o usuario {login}");
                var authInformation = _tokenService.GenerateJWTToken(user: new UserAuthInformation(
                    id: user.Id,
                    login: user.Login,
                    name: user.Name
                    ));


                user.CreateSession(ip, user.Id, authInformation);

                if (user.IsValid)
                    await DispatchEvents(user.DomainEvents);

                //var newSession = Session.CreateSession(
                //    ip: ip,
                //    userId: user.Id,
                //    expireTime: DateTime.Now.AddSeconds(authInformation.expires_in),
                //    refreshToken: authInformation.refresh_token,
                //    refreshTokenExpireTime: DateTime.Now.AddSeconds(authInformation.expires_in)
                //    );


                //if (!newSession.IsValid)
                //{
                //    _logger.LogInformation($"Autenticação do usuario {login} falhou {newSession.Notifications.CreateLogMsg()}");
                //    return Result.Failed(newSession.Notifications.CreateLogMsg());
                //}

                //await _sessionRepository.InsertAsync(newSession);

                return Result.Success(authInformation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during the login");
                return Result.Failed("Ocorreu um erro inesperado ao tentar efetuar o login");
            }
        }



        public async Task<CommandResult> CreateSession(string ip, long userId, AuthInformation authInformation)
        {
            try
            {
                _logger.LogInformation($"Criando sessão para o usuário {userId}");

                var newSession = Session.CreateSession(
                    ip: ip,
                    userId: userId,
                    expireTime: DateTime.Now.AddSeconds(authInformation.expires_in),
                    refreshToken: authInformation.refresh_token,
                    refreshTokenExpireTime: DateTime.Now.AddSeconds(authInformation.expires_in)
                    );


                if (!newSession.IsValid)
                {
                    _logger.LogInformation($"Sessão criada para o usuario {userId} invalida {newSession.Notifications.CreateLogMsg()}");
                    return Result.Failed(newSession.Notifications.CreateLogMsg());
                }

                await _sessionRepository.InsertAsync(newSession);

                return Result.Success(authInformation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while trying to create session for user {userId}");
                return Result.Failed($"Ocorreu um erro inesperado ao tentar criar sessão para o usuário {userId}");
            }
        }

        public async Task<CommandResult> RefreshToken(string refreshToken)
        {
            string userName;
            try
            {
                _logger.LogInformation($"Looking for session by refresshing token {refreshToken}");
                var session = await _sessionRepository.GetByRefreshTokenAsync(refreshToken);

                if (session == null)
                {
                    _logger.LogWarning($"Session was not found!");
                    return Result.Failed($"Refresh token invalido!");
                }
                userName = session.User?.Name;

                var validationResult = session.ValidateSession();

                if (!validationResult.IsValid)
                    return Result.Failed(validationResult.Notifications.CreateLogMsg());

                _logger.LogInformation($"Generating JWT for session {session.Id}");
                var authInformation = _tokenService.GenerateJWTToken(user: new UserAuthInformation(
                    id: session.Id,
                    login: session.User.Login,
                    name: session.User.Name
                    ));


                session.UpdateSession(
                    expireTime: DateTime.Now.AddSeconds(authInformation.expires_in),
                    newRefreshToken: authInformation.refresh_token,
                    refreshTokenExpireTime: DateTime.Now.AddSeconds(authInformation.expires_in)
                    );

                if (!session.IsValid)
                {
                    _logger.LogInformation($"Renovação sessão do usuario {userName} falhou {session.Notifications.CreateLogMsg()}");
                    return Result.Failed(session.Notifications.CreateLogMsg());
                }

                await _sessionRepository.UpdateAsync(session);

                return Result.Success(authInformation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during the session renew");
                return Result.Failed("Ocorreu um erro inesperado ao tentar renovar a sessão");
            }
        }



        #endregion

        #endregion AdministrationAppService
    }
}
