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

namespace Distvisor.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<EnvConfiguration>(Configuration.GetSection("EnvConfiguration"));

            services.AddSingleton<ICryptoService, CryptoService>();
            services.AddSingleton<IAuthCache, AuthCache>();
            services.AddScoped<IGithubService, GithubService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IInvoicesService, InvoicesService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IKeyVault, KeyVault>();

            services.AddDbContext<DistvisorContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("Sqlite")));

            services.AddControllersWithViews()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Distvisor API", Version = "v1" });
            });

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
