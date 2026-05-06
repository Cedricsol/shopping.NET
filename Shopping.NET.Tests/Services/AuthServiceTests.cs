using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Shopping.NET.DTOs;
using Shopping.NET.Exceptions;
using Shopping.NET.Models;
using Shopping.NET.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Shopping.NET.Tests.Services
{
    public class AuthServiceTests
    {
        private AuthService CreateService(AppDbContext context,
        Mock<IPasswordHasher<User>>? mockHasher = null, 
        Mock<IConfiguration>? mockConfig = null) 
        { 
            var logger = new Mock<ILogger<AuthService>>();

            var hasher = mockHasher ?? new Mock<IPasswordHasher<User>>();
            if (mockHasher == null)
            {
                hasher.Setup(h => h.HashPassword(It.IsAny<User>(), It.IsAny<string>()))
                    .Returns("hashed_password");

                hasher.Setup(h => h.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(PasswordVerificationResult.Success);
            }

            var config = mockConfig ?? new Mock<IConfiguration>();
            if (mockConfig == null)
            {
                config.Setup(c => c["Jwt:Key"]).Returns("super_secret_key_for_testing_123456789");
                config.Setup(c => c["Jwt:Issuer"]).Returns("test");
                config.Setup(c => c["Jwt:Audience"]).Returns("test");
            }
            return new AuthService(context, logger.Object, hasher.Object, config.Object);
        }

        [Fact]
        public async Task Register_Should_Add_User_To_Db_And_Return_Token()
        {
            var context = TestDbContextFactory.Create();
            var service = CreateService(context);

            var user = new RegisterDto
            {
                Email = "test@test.com",
                Username = "test",
                Password = "Test123&",
            };

            var result = await service.Register(user);

            Assert.NotNull(result);
            Assert.NotEmpty(result.Token);
            Assert.Equal(user.Email.ToLower(), result.Email);
            Assert.Equal(user.Username, result.Username);
            Assert.Equal(UserRole.User, result.Role);

            Assert.Single(context.Users);
        }

        [Fact]
        public async Task Register_Should_Normalize_Email_And_Username()
        {
            var context = TestDbContextFactory.Create();
            var service = CreateService(context);

            var registerDto = new RegisterDto
            {
                Email = " TEST@TEST.COM ",
                Username = "  TestUser  ",
                Password = "Test123&",
            };

            var result = await service.Register(registerDto);

            Assert.Equal("test@test.com", result.Email);
            Assert.Equal("testuser", result.Username);
        }

        [Fact]
        public async Task Register_Should_Throw_If_Email_Already_Exists()
        {
            var context = TestDbContextFactory.Create();
            context.Users.Add(new User
            {
                Email = "test@test.com",
                Username = "test",
            });
            await context.SaveChangesAsync();

            var service = CreateService(context);

            var user = new RegisterDto
            {
                Email = "test@test.com",
                Username = "user",
                Password = "Test123&",
            };

            await Assert.ThrowsAsync<ApiException>(() => service.Register(user));
        }

        [Fact]
        public async Task Register_Should_Throw_If_Username_Already_Exists()
        {
            var context = TestDbContextFactory.Create();
            context.Users.Add(new User
            {
                Email = "test@test.com",
                Username = "test",
            });
            await context.SaveChangesAsync();

            var service = CreateService(context);

            var user = new RegisterDto
            {
                Email = "other@test.com",
                Username = "test",
                Password = "Test123&",
            };

            await Assert.ThrowsAsync<ApiException>(() => service.Register(user));
        }

        [Fact]
        public async Task Login_Should_Return_Token_When_Credentials_Are_Valid()
        {
            var context = TestDbContextFactory.Create();
            var user = new User
            {
                Email = "test@test.com",
                Username = "test",
                PasswordHash = "hashed_password"
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            var service = CreateService(context);

            var loginDto = new LoginDto
            {
                Email = "test@test.com",
                Password = "test"
            };

            var result = await service.Login(loginDto);

            Assert.NotNull(result);
            Assert.NotEmpty(result.Token);
            Assert.Equal(user.Email, result.Email);
        }

        [Fact]
        public async Task Login_Should_Throw_User_Not_Found()
        {
            var context = TestDbContextFactory.Create();
            var service = CreateService(context);

            var loginDto = new LoginDto
            {
                Email = "notfound@test.com",
                Password = "test",
            };

            await Assert.ThrowsAsync<ApiException>(() => service.Login(loginDto));
        }

        [Fact]
        public async Task Login_Should_Throw_When_Wrong_Password()
        {
            var context = TestDbContextFactory.Create();

            var user = new User
            {
                Email = "test@test.com",
                Username = "test",
                PasswordHash = "hashed_password"
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            var mockHasher = new Mock<IPasswordHasher<User>>();
            mockHasher.Setup(h => h.VerifyHashedPassword(
                It.IsAny<User>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
                .Returns(PasswordVerificationResult.Failed);

            var service = CreateService(context, mockHasher);

            var loginDto = new LoginDto
            {
                Email = "test@test.com",
                Password = "wrong password",
            };

            await Assert.ThrowsAsync<ApiException>(() => service.Login(loginDto));
        }

        [Theory]
        [InlineData("password")] // no upper case, digit nor special caracters
        [InlineData("PASSWORD")] // no lower case, digit nor special caracters
        [InlineData("Password")] // no digit nor special caracters
        [InlineData("Password1")] // no special caracters
        public async Task Register_Should_Throw_When_Password_Is_Weak(string password)
        {
            var context = TestDbContextFactory.Create();
            var service = CreateService(context);

            var registerDto = new RegisterDto
            {
                Email = "test@test.com",
                Username = "test",
                Password = password,
            };

            await Assert.ThrowsAnyAsync<ApiException>(() => service.Register(registerDto));
        }

        [Fact]
        public async Task Register_Should_Generate_Valid_Jwt_With_Claims()
        {
            var context = TestDbContextFactory.Create();
            var service = CreateService(context);

            var registerDto = new RegisterDto
            {
                Email = "test@test.com",
                Username = "test",
                Password = "Test123&",
            };

            var result = await service.Register(registerDto);

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(result.Token);

            Assert.Contains(jwt.Claims, c => c.Type == ClaimTypes.Email && c.Value == "test@test.com");
            Assert.Contains(jwt.Claims, c => c.Type == ClaimTypes.Name && c.Value == "test");
            Assert.Contains(jwt.Claims, c => c.Type == ClaimTypes.Role && c.Value == "User");
        }

        [Fact]
        public async Task Register_Should_Throw_When_Jwt_Key_Is_Missing()
        {
            var context = TestDbContextFactory.Create();

            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["Jwt:Key"]).Returns(String.Empty);

            var service = CreateService(context, mockConfig:  mockConfig);

            var registerDto = new RegisterDto
            {
                Email = "test@test.com",
                Username = "test",
                Password = "test"
            };

            await Assert.ThrowsAsync<ApiException>(() => service.Register(registerDto));
        }

        [Fact]
        public async Task Register_Should_Assign_Default_User_Role()
        {
            var context = TestDbContextFactory.Create();
            var service = CreateService(context);

            var registerDto = new RegisterDto
            {
                Email = "test@test.com",
                Username = "test",
                Password = "Test123&",
            };

            var result = await service.Register(registerDto);

            Assert.Equal(UserRole.User, result.Role);
        }
    }
}
