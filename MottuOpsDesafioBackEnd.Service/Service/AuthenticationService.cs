using MottuOpsDesafioBackEnd.Business.Interface;
using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;

namespace MottuOpsDesafioBackEnd.Business.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationRepository _authenticationRepository;

        public AuthenticationService(IAuthenticationRepository authenticationRepository)
        {
            _authenticationRepository = authenticationRepository;
        }

        public Task<AuthenticationResponse?> PostAsync(AuthenticationRequest authenticationRequest)
        {
            try
            {
                return _authenticationRepository.PostAsync(authenticationRequest);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao inserir uma nova autenticação do usuário {authenticationRequest.Username}", ex.Message);

                throw new ApplicationException($"Erro ao inserir uma nova autenticação do usuário {authenticationRequest.Username}", ex);
            }
        }
    }
}
