using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace BookService.IntegrationTests
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Inject Test AppSettings configuration values needed by AuthService and JwtBearer setup
            builder.ConfigureAppConfiguration((context, config) =>
            {
                var testConfig = new Dictionary<string, string>
                {
                    { "AppSettings:Token", "TESTING_SECRET_KEY_THAT_IS_LONG_ENOUGH_TO_BE_SECURE_1234567890" }
                };
                config.AddInMemoryCollection(testConfig!);
            });

            // Note: DB selection is handled automatically inside Program.cs now!
        }
    }
}