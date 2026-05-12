using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shopping.NET.DTOs;
using Shopping.NET.Exceptions;
using Shopping.NET.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace Shopping.NET.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<AuthService> _logger;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context,  ILogger<AuthService> logger, IPasswordHasher<User> hasher, IConfiguration configuration)
        {
            _dbContext = context;
            _logger = logger;
            _passwordHasher = hasher;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> Register(RegisterDto registerDto)
        {
            var email = registerDto.Email.ToLower().Trim();
            var username = registerDto.Username.ToLower().Trim();
            // Check if user already exists
            if (await _dbContext.Users.AnyAsync(u => u.Email == email))
            {
                throw new ApiException("Email déjà utilisé", "EMAIL_ALREADY_USED", 400);
            }

            if (await _dbContext.Users.AnyAsync(u => u.Username == username))
            {
                throw new ApiException("Nom d'utilisateur déjà pris", "USERNAME_ALREADY_TAKEN", 400);
            }

            // Password validation
            if (!Regex.IsMatch(registerDto.Password, @"[A-Z]") ||
                !Regex.IsMatch(registerDto.Password, @"[a-z]") ||
                !Regex.IsMatch(registerDto.Password, @"[0-9]") ||
                !Regex.IsMatch(registerDto.Password, @"[!@#$%^&*/]"))
            {
                throw new ApiException("Mot de passe trop faible", "WEAK_PASSWORD", 400);
            }

            // Then create a new user
            var user = new User
            {
                Username = username,
                Email  = email,
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, registerDto.Password);

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("User created with email : {Email}", user.Email);
            var token = GenerateJwtToken(user);
            _logger.LogInformation("Token created");

            return new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                Username = user.Username,
                Role = user.Role
            };
        }

        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email.ToLower().Trim());
            if (user == null)
            {
                throw new ApiException("Email ou mot de passe incorrect", "INVALID_CREDENTIALS", 401);
            }
            var passwordCheck = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
            if (passwordCheck == PasswordVerificationResult.Failed)
            {
                // Wrong password
                _logger.LogWarning("Failed login attempt for email {Email}", loginDto.Email);
                throw new ApiException("Email ou mot de passe incorrect", "INVALID_CREDENTIALS", 401);

            }
            _logger.LogInformation("User logged in : {Email}", user.Email);
            var token = GenerateJwtToken(user);
            _logger.LogInformation("Token created");

            return new AuthResponseDto 
            {
                Token = token,
                Email = user.Email,
                Username = user.Username,
                Role = user.Role
            };
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString() ?? "User"),
            };
            var keyString = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(keyString))
            {
                throw new ApiException("JWT Key is missing", "JWT_KEY_MISSING", 400);
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
