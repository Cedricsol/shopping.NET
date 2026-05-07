using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Shopping.NET.Tests.Authorizations
{
    public class AuthorizationTests
    {
        [Fact]
        public async Task RegisterProduct_Should_Return_401_When_Not_Authorized()
        {
            var factory = new CustomWebApplicationFactory();
            var client = factory.CreateClient();

            var response = await client.PostAsync("/api/products", null);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task RegisterProduct_Should_Return_403_When_User_Is_Not_Admin()
        {
            var factory = new CustomWebApplicationFactory();
            var client = factory.CreateClient();

            var token = JwtTestHelper.GenerateToken("User");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync("/api/products", null);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task RegisterProduct_Should_Work_When_User_Is_Admin()
        {
            var factory = new CustomWebApplicationFactory();
            var client = factory.CreateClient();

            var token = JwtTestHelper.GenerateToken("Admin");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(
                JsonSerializer.Serialize(new { name = "Test Product", price = 10, imageUrl = "test"}),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync("/api/products", content);

            var body = await response.Content.ReadAsStringAsync();

            Console.WriteLine( body );

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
