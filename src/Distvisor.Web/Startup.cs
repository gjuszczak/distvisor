using Distvisor.Infrastructure;
using Distvisor.Web.Configuration;
using Distvisor.Web.Data;
using Distvisor.Web.Filters;
using Distvisor.Web.Hubs;
using Distvisor.Web.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using System.Text.Json.Serialization;

namespace Distvisor.Web
{
    public class Startup
    {
        public Startup(IConfiguration config)
        {
            Config = config;
        }

        public IConfiguration Config { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ClientConfiguration>(Config.GetSection("ClientConfig"));
            services.Configure<AzureAdConfiguration>(Config.GetSection("AzureAd"));
            services.Configure<DeploymentConfiguration>(Config.GetSection("Deployment"));
            services.Configure<MailgunConfiguration>(Config.GetSection("Mailgun"));
            services.Configure<FinancesConfiguration>(Config.GetSection("Finances"));
            services.Configure<EwelinkConfiguration>(Config.GetSection("Ewelink"));
            services.Configure<RfLinkConfiguration>(Config.GetSection("RfLink"));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ICryptoService, CryptoService>();
            services.AddSingleton<IEventLogToDtoMapper, EventLogToDtoMapper>();
            services.AddSingleton<IFinancialDataExtractor, FinancialCsvDataExtractor>();
            services.AddSingleton<IFinancialCsvDataExtractor, CsvSVariantDataExtractor>();
            services.AddSingleton<IFinancialCsvDataExtractor, CsvIVariantDataExtractor>();
            services.AddSingleton<IEwelinkClientWebSocketFactory, EwelinkClientWebSocketFactory>();
            services.AddSingleton<IAuthTokenCacheFactory, AuthTokenCacheFactory>();
            services.AddScoped<IHomeBoxService, HomeBoxService>();
            services.AddScoped<IDeploymentService, DeploymentService>();
            services.AddScoped<IBackupService, BackupService>();
            services.AddScoped<ISecretsVault, SecretsVault>();
            services.AddScoped<IUserInfoProvider, UserInfoProvider>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IRedirectionsService, RedirectionsService>();
            services.AddScoped<IBackupProcessManager, BackupProcessManager>();
            services.AddScoped<IFinancialService, FinancialService>();

            services.AddHttpClient<IMailgunClient, MailgunClient, FakeMailgunClient>(Config)
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri("https://api.eu.mailgun.net/");
                    c.DefaultRequestHeaders.Add("Accept", "application/json");
                });

            services.AddHttpClient<IGithubClient, GithubClient, FakeGithubClient>(Config)
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri("https://api.github.com/");
                    c.DefaultRequestHeaders.Add("User-Agent", "distvisor");
                    c.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
                });

            services.AddHttpClient<IOneDriveClient, OneDriveClient>()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri("https://graph.microsoft.com/");
                    c.DefaultRequestHeaders.Add("Accept", "application/json");
                });

            services.AddHttpClient<IEwelinkClient, EwelinkClient>()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri(Config.GetValue<string>("Ewelink:ApiUrl"));
                    c.DefaultRequestHeaders.Add("Accept", "application/json");
                });

            services.AddHttpClient<IMicrosoftAuthClient, MicrosoftAuthClient>()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri(Config.GetValue<string>("AzureAd:Instance"));
                    c.DefaultRequestHeaders.Add("Accept", "application/json");
                });

            services.AddHttpClient<IOneDriveUploadSessionFactory, OneDriveUploadSessionFactory>();

            services.AddEventSourcing(Config.GetConnectionString("EventStore"), Config.GetConnectionString("ReadStore"));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Distvisor API", Version = "v1" });
                c.DescribeAllParametersInCamelCase();
            });

            services.AddSignalR();

            services.AddDistvisorAuth(Config);

            services.AddMvc(opts =>
            {
                opts.Filters.Add<ApiExceptionFilterAttribute>();
            })
            .AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            })
            .AddFluentValidation(opts =>
            {
                opts.AutomaticValidationEnabled = false;
            });

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            // Configure new, cqrs based, Distvisor services
            services.AddDistvisor(Config);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var pwaProvider = new FileExtensionContentTypeProvider();
            pwaProvider.Mappings[".webmanifest"] = "application/manifest+json";

            if (env.IsDevelopment())
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

            app.UseAccessCookie(Config.GetValue<string>("AccessCookie"));

            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = pwaProvider
            });

            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles(new StaticFileOptions
                {
                    ContentTypeProvider = pwaProvider
                });
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHub<NotificationsHub>("/hubs/notificationshub");

                endpoints.MapRazorPages();
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
