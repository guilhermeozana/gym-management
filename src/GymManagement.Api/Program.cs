using GymManagement.Application;
using GymManagement.Infrastructure;
using GymManagement.Infrastructure.Common.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();
    builder.Services.AddControllers();
    builder.Services.AddProblemDetails();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddHttpContextAccessor();
    builder.Services
        .AddApplication()
        .AddInfrastructure(builder.Configuration);
}

var app = builder.Build();

{
    app.UseExceptionHandler();
    app.AddInfrastructureMiddleware();
    
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseHttpsRedirection();

    app.MapControllers();
    app.UseAuthorization();
    app.Run();
}
