// Startup.cs by Gradzka & Kazimierczak

namespace LeonCam2
{
    using System.Data;
    using System.Data.SQLite;
    using System.Globalization;
    using LeonCam2.Extensions;
    using LeonCam2.Filters.AuthorizationFilters;
    using LeonCam2.Models;
    using LeonCam2.Repositories;
    using LeonCam2.Services;
    using LeonCam2.Services.JwtTokens;
    using LeonCam2.Services.Users;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;

    public class Startup
    {
        private readonly CultureInfo[] supportedCultures = new[]
        {
            new CultureInfo("en-US"),
        };

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var settingsSection = this.Configuration.GetSection("Settings");
            services.Configure<Settings>(settingsSection);

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(x => x.SwaggerDoc("v1", new OpenApiInfo { Title = "LeonCam2 API", Version = "v1" }));

            services.AddControllersWithViews();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration => configuration.RootPath = "ClientApp/build");

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddTransient<IDbConnection>((_) => new SQLiteConnection(this.Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IUserService, UserService>();

            services.AddSingleton<IJwtTokenService, JwtTokenService>();

            services.AddScoped<JwtTokenFilter>();

            services.AddHostedService<TimedBackgroundService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = this.supportedCultures,
                SupportedUICultures = this.supportedCultures,
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));

            app.ConfigureCustomExceptionMiddleware();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

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
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
