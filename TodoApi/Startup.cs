using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using GraphQL.Server;
using GraphQL.Types;
using TodoApi.Graphql;
using GraphQL.DataLoader;

namespace TodoApi
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
            services.AddControllers();
            services.AddDbContext<ApplicationDbContext>(opt =>
              opt.UseNpgsql(Configuration.GetConnectionString("PostgresConnection"))
              .UseSnakeCaseNamingConvention());
            #region identityUser

            services.AddDefaultIdentity<Usuario>(Opt => Opt.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });
            services.AddRazorPages();
            #endregion
            
            services.AddSwaggerGen();


            #region graphql
            services.AddScoped<IGraphStore<Car>,CarGraphStore>();
            services.AddScoped<IGraphStore<Brand>,BrandGraphStore>();
            
            services.AddScoped<WebQuery>();
            services.AddScoped<ISchema, AppSchema>();

            services.AddSingleton<IDataLoaderContextAccessor,DataLoaderContextAccessor>();
            services.AddSingleton<DataLoaderDocumentListener>();

            services.AddLogging(builder => builder.AddConsole());
            services.AddHttpContextAccessor();
            services.AddGraphQL(options =>
            {
                options.EnableMetrics = true;
                options.ExposeExceptions = true;
            })
                .AddSystemTextJson()
                .AddDataLoader()
                .AddGraphTypes(ServiceLifetime.Scoped);
            #endregion
            
            #region Servicios Autenticacion
            var key = Encoding.ASCII.GetBytes(Configuration["IssuerKey"]);
            services.AddAuthentication()
            .AddJwtBearer("Bearer", x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidIssuer = Configuration["Issuer"]
                };
            });
            #endregion


        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
        
            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseGraphQL<ISchema>();
            app.UseGraphQLPlayground();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}
