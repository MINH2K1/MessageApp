using MessageApplication.ViewModel;

namespace MessageApplication.Service.Interface
{
    public interface IAuthService
    {
        string GenerateTokenString(LoginViewModel login);
        Task<bool> RegisterUser(RegisterViewModel register);
        Task<bool> Login(LoginViewModel login);
    }
}
