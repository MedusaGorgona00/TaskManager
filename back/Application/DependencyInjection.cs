using Application.Common.Behaviours;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Application
{
    public static class DependencyInjection
    {
        /// <summary>
        ///     Add all dependencies from Application
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddMediatR(assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            return services;
        }
    }


    public static class JsonInstaller
    {
        public static IMvcBuilder AddJsonOptions(this IMvcBuilder builder)
        {
            builder.AddJsonOptions(c =>
            {
                c.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                c.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(null, true));
            });

            return builder;
        }
    }

}
