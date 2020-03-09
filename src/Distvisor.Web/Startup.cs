using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Distvisor.Web.Data;
using Distvisor.Web.Services;
using System.Text.Json.Serialization;
using Distvisor.Web.Configuration;
using Microsoft.AspNetCore.Http;
using Distvisor.Web.Hubs;
using System;

namespace Distvisor.Web
{
    public class Startup
    {
        public Startup(IConfiguration config, IWebHostEnvironment env)
        {
            Config = config;
            Env = env;
        }

        public IConfiguration Config { get; }
        public IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<EnvConfiguration>(Config.GetSection("EnvConfiguration"));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ICryptoService, CryptoService>();
            services.AddSingleton<IAuthCache, AuthCache>();
            services.AddScoped<IGithubService, GithubService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IInvoicesService, InvoicesService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISecretsVault, SecretsVault>();
            services.AddScoped<IMicrosoftAuthService, MicrosoftAuthService>();
            services.AddScoped<IMicrosoftAuthTokenStore, MicrosoftAuthTokenStore>();
            services.AddScoped<IMicrosoftOneDriveService, MicrosoftOneDriveService>();
            services.AddScoped<IUserInfoProvider, UserInfoProvider>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<INotificationStore, NotificationStore>();

            services.AddProdOrDevHttpClient<IMailgunClient, MailgunClient, FakeMailgunClient>(Env, Config)
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.eu.mailgun.net/"));

            services.AddDbContext<DistvisorContext>(options =>
            options.UseSqlite(Config.GetConnectionString("Sqlite")));

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

            services.AddDistvisorAuth();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DistvisorContext context)
        {
            context.Database.Migrate();

            var pwaProvider = new FileExtensionContentTypeProvider();
            pwaProvider.Mappings[".webmanifest"] = "application/manifest+json";

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();

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

                endpoints.MapHub<NotificationsHub>("/notificationshub");
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
