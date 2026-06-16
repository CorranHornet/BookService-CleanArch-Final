using BookService.Application.Common.Behaviors;
using BookService.Application.Common.Mapping;
using FluentValidation;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BookService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        //Console.WriteLine("VALIDATORS REGISTER START");
        //services.AddValidatorsFromAssembly(typeof(CreateMediaItemCommandValidator).Assembly);
        //Console.WriteLine("VALIDATORS REGISTER END");


        var assembly = Assembly.GetExecutingAssembly();

        // Register MediatR and Pipeline Behaviors
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        // Register FluentValidation
        services.AddValidatorsFromAssembly(assembly);

        // Register Mapster as a Service for consistent DTO mapping
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(assembly);
        services.AddSingleton<IMapper>(new Mapper(config));

        // Register custom mapping configurations
        MapsterConfig.Register();

        return services;
    }
}