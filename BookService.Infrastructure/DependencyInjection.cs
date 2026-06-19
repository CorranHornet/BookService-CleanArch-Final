using BookService.Application.Interfaces;
using BookService.Infrastructure.Persistence;
using BookService.Infrastructure.Repositories;
using BookService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Setup DbContext based on environment
            var isTestEnvironment = AppDomain.CurrentDomain.GetAssemblies()
                .Any(a => a.FullName!.Contains("IntegrationTests"));

            if (isTestEnvironment)
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("LibraryIntegrationTestDatabase"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            }

            // Register IApplicationDbContext
            services.AddScoped<IApplicationDbContext>(provider =>
                provider.GetRequiredService<ApplicationDbContext>());

            // Register Repositories
            services.AddScoped<IMediaItemRepository, MediaItemRepository>();
            services.AddScoped<IMediaUnitRepository, MediaUnitRepository>();
            services.AddScoped<ILoanRepository, LoanRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();

            // Add Auth Service
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}