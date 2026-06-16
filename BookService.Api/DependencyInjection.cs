namespace BookService.Api;
public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddControllers();

        // Swagger documentation setup
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }
}