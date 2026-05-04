using Microsoft.AspNetCore.Mvc;
using Moq;
using Shopping.NET.Controllers;
using Shopping.NET.DTOs;
using Shopping.NET.Models;
using Shopping.NET.Services;

namespace Shopping.NET.Tests.Controllers
{
    public class AuthControllerTests
    {
        [Fact]
        public async Task Login_Should_Return_Ok()
        {
            // Setup
            var mockService = new Mock<IAuthService>();

            var user = new LoginDto
            {
                Email = "test@test.com",
                Password = "test"
            };

            var returnedDto = new AuthResponseDto
            {
                Token = "token",
                Email = "test@test.com",
                Username = "test",
                Role = UserRole.User,
            };

            mockService
                .Setup(s => s.Login(user))
                .ReturnsAsync(returnedDto);

            var controller = new AuthController(mockService.Object);


            var result = await controller.Login(user);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedAuthResponse = Assert.IsType<AuthResponseDto>(okResult.Value);

            Assert.Equal("token", returnedAuthResponse.Token);
            Assert.Equal("test@test.com", returnedAuthResponse.Email);
            Assert.Equal("test", returnedAuthResponse.Username);
            Assert.Equal(UserRole.User, returnedAuthResponse.Role);
        }

        [Fact]
        public async Task Register_Should_Return_Ok()
        {
            // Setup
            var mockService = new Mock<IAuthService>();

            var user = new RegisterDto
            {
                Email = "test@test.com",
                Password = "test",
                Username = "test",
            };

            var returnedDto = new AuthResponseDto
            {
                Token = "token",
                Email = "test@test.com",
                Username = "test",
                Role = UserRole.User,
            };

            mockService
                .Setup(s => s.Register(user))
                .ReturnsAsync(returnedDto);

            var controller = new AuthController(mockService.Object);

            var result = await controller.Register(user);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedAuthResponse = Assert.IsType<AuthResponseDto>(okResult.Value);

            Assert.Equal("token", returnedAuthResponse.Token);
            Assert.Equal("test@test.com", returnedAuthResponse.Email);
            Assert.Equal("test", returnedAuthResponse.Username);
            Assert.Equal(UserRole.User, returnedAuthResponse.Role);
        }
    }
}
