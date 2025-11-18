using SuperEsperanzaFrontEnd.Modelos.Dto;

namespace SuperEsperanzaFrontEnd.Repositorio.Interfaces
{
    public interface IAuthRepository
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
    }
}

