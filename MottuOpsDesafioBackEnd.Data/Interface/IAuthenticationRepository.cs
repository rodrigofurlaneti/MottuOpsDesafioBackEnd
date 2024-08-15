using MottuOpsDesafioBackEnd.Domain.Models;

namespace MottuOpsDesafioBackEnd.Data.Interface
{
    public interface IAuthenticationRepository
    {
        Task<AuthenticationResponse?> PostAsync(AuthenticationRequest authenticationRequest);
    }
}
