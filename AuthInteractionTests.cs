using Auth.DTO;

namespace Auth.Auth.InteractionTests
{
    public class AuthInteractionTests
    {
        private readonly HttpClient _httpClient;
        
        public AuthInteractionTests(HttpClient httpClient)
        {
            _httpClient = httpClient; 
        }

        [Fact]
        public async Task Register_Return_Token()
        {
            //Arrange - preparing for test 
            var req = new RegisterRequest
            {
                Email = "Chimin@testmail.com",
                Password = "password",
            };
            //Act - act of test

            var resp = await _httpClient.PostAsJsonAsync("/api/auth/register", req);
            //Assert - check of test

            Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
            var body = await resp.Content.ReadFromJsonAsync<AuthResponse>();
            Assert.NotNull(body);
            Assert.False(string.IsNullOrEmpty(body.Accesstoken));
            Assert.False(string.IsNullOrEmpty(body.RefreshToken));

            
        }
    }
}
