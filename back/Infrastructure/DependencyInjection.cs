using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using AutoMapper.EquivalencyExpression;
using Microsoft.Extensions.Logging;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration, ILoggingBuilder logging)
        {
            services.AddDbCtx(configuration);

            var assembly = Assembly.GetExecutingAssembly();
            services.AddAutoMapper(cfg =>
            {
                cfg.AddCollectionMappers();
            },assembly );

            return services;
        }

        public static IServiceCollection AddDbCtx(this IServiceCollection services, IConfiguration configuration)
        {
            var z = configuration.GetConnectionString("Default");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("Default"),
                    b =>
                    {
                        b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                        b.UseNetTopologySuite();
                    }));
            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            return services;
        }
    }
}
