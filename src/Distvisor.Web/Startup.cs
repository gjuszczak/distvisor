using Distvisor.Web.BackgroundServices;
using Distvisor.Web.Configuration;
using Distvisor.Web.Data;
using Distvisor.Web.Hubs;
using Distvisor.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
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

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IEventLogToDtoMapper, EventLogToDtoMapper>();
            services.AddSingleton<IEmailReceivedNotifier, EmailReceivedNotifier>();
            services.AddSingleton<IFinancialEmailDataExtractor, FinancialEmailDataExtractor>();
            services.AddSingleton<IFinancialEmailAnalyzer, AccountIncomeEmailAnalyzer>();
            services.AddSingleton<IFinancialEmailAnalyzer, AccountDebtEmailAnalyzer>();
            services.AddSingleton<IFinancialEmailAnalyzer, CardPaymentEmailAnalyzer>();
            services.AddScoped<IDeploymentService, DeploymentService>();
            services.AddScoped<IBackupService, BackupService>();
            services.AddScoped<IInvoicesService, InvoicesService>();
            services.AddScoped<IEmailSendingService, EmailSendingService>();
            services.AddScoped<IEmailReceivingService, EmailReceivingService>();
            services.AddScoped<ISecretsVault, SecretsVault>();
            services.AddScoped<IMicrosoftAuthService, MicrosoftAuthService>();
            services.AddScoped<IUserInfoProvider, UserInfoProvider>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IRedirectionsService, RedirectionsService>();
            services.AddScoped<IBackupProcessManager, BackupProcessManager>();
            services.AddScoped<IEmailFileImportService, EmailFileImportService>();

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

            services.AddHttpClient<IIFirmaClient, IFirmaClient, FakeIFirmaClient>(Config)
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri("https://www.ifirma.pl/");
                    c.DefaultRequestHeaders.Add("Accept", "application/json");
                });

            services.AddHttpClient<IOneDriveClient, OneDriveClient>()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri("https://graph.microsoft.com/");
                    c.DefaultRequestHeaders.Add("Accept", "application/json");
                });

            services.AddEventSourcing(Config.GetConnectionString("EventStore"), Config.GetConnectionString("ReadStore"));

            services.AddControllersWithViews()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Distvisor API", Version = "v1" });
            });

            services.AddSignalR();

            services.AddDistvisorAuth(Config);

            services.AddHostedService<EmailPoolingBackgroundService>();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
            app.UseDoNotRedirectCookie();

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
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");

                endpoints.MapHub<NotificationsHub>("/hubs/notificationshub");
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
