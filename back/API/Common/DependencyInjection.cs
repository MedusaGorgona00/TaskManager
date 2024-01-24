using Microsoft.OpenApi.Models;
using System.Reflection;

namespace API.Common
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSwaggerDependency(this IServiceCollection services, Assembly executingAssembly)
        {
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "Test service api",
                        Version = "v1"
                    });

                    c.DescribeAllParametersInCamelCase();

                    c.SupportNonNullableReferenceTypes();
                });

                return services;
            }
        }
    }
}
