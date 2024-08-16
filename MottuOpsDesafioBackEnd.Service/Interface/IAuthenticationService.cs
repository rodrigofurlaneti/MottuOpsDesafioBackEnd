using MottuOpsDesafioBackEnd.Domain.Models;

namespace MottuOpsDesafioBackEnd.Business.Interface
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse?> PostAsync(AuthenticationRequest authenticationRequest);
    }
}
