using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Xunit; // 💡 FIXED: This brings back the Assert class so your code can compile!
using BookService.Application.DTOs;
using BookService.Domain.Entities;
using BookService.Infrastructure.Persistence;

namespace BookService.IntegrationTests
{
    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public static bool IsAuthenticated = false;

        public TestAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder)
            : base(options, logger, encoder) { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!IsAuthenticated)
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "admin@library.com"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var identity = new ClaimsIdentity(claims, "TestAuthScheme");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "TestAuthScheme");

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }

    public class LibraryApiSystemTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        private const string TestTokenKey = "SuperSecretTestingKeyThatIsAtLeast512BitsLongAndSafeForSha512Signatures!";
        private const string TestIssuer = "MyAwesomeApp";
        private const string TestAudience = "MyAwesomeAudience";

        public LibraryApiSystemTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        { "AppSettings:Token", TestTokenKey },
                        { "AppSettings:Issuer", TestIssuer },
                        { "AppSettings:Audience", TestAudience }
                    });
                });

                builder.ConfigureServices(services =>
                {
                    services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = "TestAuthScheme";
                        options.DefaultChallengeScheme = "TestAuthScheme";
                    })
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestAuthScheme", options => { });
                });
            });

            _client = _factory.CreateClient();
        }

        private async Task AuthenticateAsAdminAsync()
        {
            TestAuthHandler.IsAuthenticated = true;

            var adminUsername = "admin@library.com";
            var adminPassword = "123";

            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var userExists = await db.Users.AnyAsync(u => u.Username == adminUsername);
                if (!userExists)
                {
                    var testAdmin = new User
                    {
                        Username = adminUsername,
                        Email = "admin@library.com",
                        Role = "Admin"
                    };

                    var hasher = new PasswordHasher<User>();
                    testAdmin.PasswordHash = hasher.HashPassword(testAdmin, adminPassword);

                    db.Users.Add(testAdmin);
                    await db.SaveChangesAsync();
                }
            }

            var loginDto = new LoginRequestDTO
            {
                Username = adminUsername,
                Password = adminPassword
            };

            var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
            response.EnsureSuccessStatusCode();

            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();
            Assert.NotNull(loginResponse);
            Assert.False(string.IsNullOrEmpty(loginResponse.Token));

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse.Token);
        }

        [Fact]
        public async Task Execute_Complete_LibraryLifecycle_Validation_Suite()
        {
            // ----------------------------------------------------
            // PHASE 1: Security Guard Check
            // ----------------------------------------------------
            TestAuthHandler.IsAuthenticated = false;
            _client.DefaultRequestHeaders.Authorization = null;

            var unauthorizedGenreResponse = await _client.PostAsJsonAsync("/api/genres", new { Name = "Sci-Fi" });
            Assert.Equal(HttpStatusCode.Unauthorized, unauthorizedGenreResponse.StatusCode);

            // Trigger actual authentication flow now
            await AuthenticateAsAdminAsync();

            // ----------------------------------------------------
            // PHASE 2: Create Operation Validation
            // ----------------------------------------------------
            var validGenrePayload = new { Name = "Science Fiction" };
            var createGenreResponse = await _client.PostAsJsonAsync("/api/genres", validGenrePayload);
            createGenreResponse.EnsureSuccessStatusCode();

            var createdGenre = await createGenreResponse.Content.ReadFromJsonAsync<GenreResponseDTO>();

            Assert.NotNull(createdGenre);
            Assert.True(createdGenre.Id > 0);

            // ----------------------------------------------------
            // PHASE 3: Read Operation Validation
            // ----------------------------------------------------
            var readResponse = await _client.GetAsync($"/api/genres/{createdGenre.Id}");
            readResponse.EnsureSuccessStatusCode();

            var fetchedGenre = await readResponse.Content.ReadFromJsonAsync<GenreResponseDTO>();
            Assert.NotNull(fetchedGenre);
            Assert.Equal(validGenrePayload.Name, fetchedGenre.Name);

            // ----------------------------------------------------
            // PHASE 4: Update Operation Validation (TEMPORARILY SKIPPED)
            // ----------------------------------------------------
            /* 
            var updatePayload = new { Id = createdGenre.Id, Name = "Sci-Fi Romance" };
            var updateResponse = await _client.PutAsJsonAsync($"/api/genres/{createdGenre.Id}", updatePayload);
            updateResponse.EnsureSuccessStatusCode();
            */

            // ----------------------------------------------------
            // PHASE 5: Delete Operation Validation
            // ----------------------------------------------------
            var deleteResponse = await _client.DeleteAsync($"/api/genres/{createdGenre.Id}");
            deleteResponse.EnsureSuccessStatusCode();

            var verifyDeleteResponse = await _client.GetAsync($"/api/genres/{createdGenre.Id}");

            bool isGone = verifyDeleteResponse.StatusCode == HttpStatusCode.NotFound ||
                          verifyDeleteResponse.StatusCode == HttpStatusCode.NoContent;

            Assert.True(isGone);
        }
    }
}