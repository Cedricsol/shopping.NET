using Shopping.NET.DTOs;

namespace Shopping.NET.Services
{
    public interface IAuthService
    {
        Task<UserDto> Register(RegisterDto registerDto);
        Task<AuthResponseDto> Login(LoginDto loginDto);
    }
}
