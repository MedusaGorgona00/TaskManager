using Application;
using API.Common.Middlewares;
using Infrastructure;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;
using API.Common;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var logging = builder.Logging;
logging.ClearProviders();

builder.Services.AddControllers().AddJsonOptions();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization(options =>
{

});

builder.Services.AddInfrastructure(configuration, logging)
    .AddApplicationDependencies()
    .AddSwaggerDependency(Assembly.GetExecutingAssembly());

builder.Services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>("database");

builder.Services.AddHttpContextAccessor();
var app = builder.Build();

await DatabaseMigrator.SeedDatabaseAsync(app.Services);

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// Configure the HTTP request pipeline.

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DisplayRequestDuration();
        c.SwaggerEndpoint($"/swagger/v1/swagger.json", "Test.Api v1");
        c.DocumentTitle = "Test service API Documentation";

        c.DefaultModelRendering(ModelRendering.Example);
        c.DefaultModelsExpandDepth(-1);
        c.DisplayOperationId();
        c.DocExpansion(DocExpansion.None);
        c.EnableDeepLinking();
        c.EnableFilter();
        c.ShowExtensions();
    });
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHealthChecks("/health");
//app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.Run();
