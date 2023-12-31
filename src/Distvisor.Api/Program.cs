using Distvisor.App.Core.Serialization;
using Distvisor.Infrastructure;
using Distvisor.Api.Configuration;
using Distvisor.Api.Services;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Distvisor.App.Features.Common.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ClientConfiguration>(builder.Configuration.GetSection("ClientConfig"));
builder.Services.Configure<AzureAdConfiguration>(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IUserInfoProvider, UserInfoProvider>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Distvisor API", Version = "v1" });
    c.DescribeAllParametersInCamelCase();
});

builder.Services.ConfigureHttpJsonOptions(opts =>
{
    JsonDefaults.Configure(opts.SerializerOptions);
});

//builder.Services.AddDistvisorAuth(builder.Configuration);
builder.Services.AddDistvisor(builder.Configuration);
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddHostedService<DbMigrationsBootstrap>();
builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.Use(async (context, next) =>
    {
        //throtle
        await Task.Delay(500);
        await next.Invoke();
    });
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Distvisor API V1");
    });
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
