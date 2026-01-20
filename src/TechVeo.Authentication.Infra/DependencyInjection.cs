using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechVeo.Authentication.Application;
using TechVeo.Authentication.Domain.Repositories;
using TechVeo.Authentication.Infra.Persistence.Contexts;
using TechVeo.Authentication.Infra.Persistence.Repositories;
using TechVeo.Shared.Infra.Extensions;

namespace TechVeo.Authentication.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfra(this IServiceCollection services)
    {
        services.AddSharedInfra<AuthContext>(new InfraOptions
        {
            DbContext = (serviceProvider, dbOptions) =>
            {
                var config = serviceProvider.GetRequiredService<IConfiguration>();
                dbOptions.UseSqlServer(config.GetConnectionString("DataBaseConection"));
            },
            ApplicationAssembly = typeof(DependecyInjection).Assembly
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IServiceClientRepository, ServiceClientRepository>();

        //MediatR
        services.AddMediatR(typeof(DependecyInjection));

        return services;
    }
}
