using Microsoft.EntityFrameworkCore;
using Distvisor.App.Core.Serialization;
using Distvisor.Infrastructure;
using Distvisor.Web.Configuration;
using Distvisor.Web.Data;
using Distvisor.Web.Filters;
using Distvisor.Web.Hubs;
using Distvisor.Web.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.OpenApi.Models;
using System.Text;
using Distvisor.Web;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ClientConfiguration>(builder.Configuration.GetSection("ClientConfig"));
builder.Services.Configure<AzureAdConfiguration>(builder.Configuration.GetSection("AzureAd"));
builder.Services.Configure<DeploymentConfiguration>(builder.Configuration.GetSection("Deployment"));
builder.Services.Configure<MailgunConfiguration>(builder.Configuration.GetSection("Mailgun"));
builder.Services.Configure<FinancesConfiguration>(builder.Configuration.GetSection("Finances"));
builder.Services.Configure<EwelinkConfiguration>(builder.Configuration.GetSection("Ewelink"));
builder.Services.Configure<RfLinkConfiguration>(builder.Configuration.GetSection("RfLink"));
        
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<ICryptoService, CryptoService>();
builder.Services.AddSingleton<IEventLogToDtoMapper, EventLogToDtoMapper>();
builder.Services.AddSingleton<IFinancialDataExtractor, FinancialCsvDataExtractor>();
builder.Services.AddSingleton<IFinancialCsvDataExtractor, CsvSVariantDataExtractor>();
builder.Services.AddSingleton<IFinancialCsvDataExtractor, CsvIVariantDataExtractor>();
builder.Services.AddSingleton<IEwelinkClientWebSocketFactory, EwelinkClientWebSocketFactory>();
builder.Services.AddSingleton<IAuthTokenCacheFactory, AuthTokenCacheFactory>();
builder.Services.AddScoped<IHomeBoxService, HomeBoxService>();
builder.Services.AddScoped<IDeploymentService, DeploymentService>();
builder.Services.AddScoped<IBackupService, BackupService>();
builder.Services.AddScoped<ISecretsVault, SecretsVault>();
builder.Services.AddScoped<IUserInfoProvider, UserInfoProvider>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IRedirectionsService, RedirectionsService>();
builder.Services.AddScoped<IBackupProcessManager, BackupProcessManager>();
builder.Services.AddScoped<IFinancialService, FinancialService>();

builder.Services.AddHttpClient<IMailgunClient, MailgunClient, FakeMailgunClient>(builder.Configuration)
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri("https://api.eu.mailgun.net/");
        c.DefaultRequestHeaders.Add("Accept", "application/json");
    });

builder.Services.AddHttpClient<IGithubClient, GithubClient, FakeGithubClient>(builder.Configuration)
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri("https://api.github.com/");
        c.DefaultRequestHeaders.Add("User-Agent", "distvisor");
        c.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
    });

builder.Services.AddHttpClient<IOneDriveClient, OneDriveClient>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri("https://graph.microsoft.com/");
        c.DefaultRequestHeaders.Add("Accept", "application/json");
    });

builder.Services.AddHttpClient<IEwelinkClient, EwelinkClient>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("Ewelink:ApiUrl"));
        c.DefaultRequestHeaders.Add("Accept", "application/json");
    });

builder.Services.AddHttpClient<IMicrosoftAuthClient, MicrosoftAuthClient>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("AzureAd:Instance"));
        c.DefaultRequestHeaders.Add("Accept", "application/json");
    });

builder.Services.AddHttpClient<IOneDriveUploadSessionFactory, OneDriveUploadSessionFactory>();

builder.Services.AddEventSourcing(
    builder.Configuration.GetConnectionString("EventStore"),
    builder.Configuration.GetConnectionString("ReadStore"));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Distvisor API", Version = "v1" });
    c.DescribeAllParametersInCamelCase();
});

builder.Services.AddSignalR();

builder.Services.AddDistvisorAuth(builder.Configuration);

builder.Services.AddMvc(opts =>
{
    opts.Filters.Add<ApiExceptionFilterAttribute>();
})
.AddJsonOptions(opts =>
{
    JsonDefaults.Configure(opts.JsonSerializerOptions);
})
.AddFluentValidation(opts =>
{
    opts.AutomaticValidationEnabled = false;
});

builder.Services.AddDistvisor(builder.Configuration);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Distvisor API V1");
    });
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseAccessCookie(app.Configuration.GetValue<string>("AccessCookie"));

var pwaProvider = new FileExtensionContentTypeProvider();
pwaProvider.Mappings[".webmanifest"] = "application/manifest+json";
app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = pwaProvider
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationsHub>("/hubs/notificationshub");
app.MapRazorPages();
app.MapFallbackToFile("index.html");

app.Run();
