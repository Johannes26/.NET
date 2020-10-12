using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using GraphQL.Authorization;
using GraphQL.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using GraphQL.Server.Ui.Playground;

namespace TodoApi.Graphql
{
    public static class ServiceConfigurationExtensions
    {
        public static void AddGraphQLAuth(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IAuthorizationEvaluator, AuthorizationEvaluator>();
            services.AddTransient<IValidationRule, AuthorizationValidationRule>();

            services.TryAddSingleton(s =>
            {
                var authSettings = new AuthorizationSettings();

                authSettings.AddPolicy("AdminPolicy", _ => _.RequireClaim("role", "Admin"));

                return authSettings;
            });
        }


        public static void UseGraphQLWithAuth(this IApplicationBuilder app)
        {
            var settings = new GraphQLSettings
            {
                BuildUserContext = async ctx =>
                {
                    var userContext = new GraphQLUserContext
                    {
                        User = ctx.User
                    };

                    return await Task.FromResult(userContext);
                }
            };

            var rules = app.ApplicationServices.GetServices<IValidationRule>();
            settings.ValidationRules.AddRange(rules);

            app.UseMiddleware<PlaygroundMiddleware>(settings);
        }

        public class GraphQLUserContext : IProvideClaimsPrincipal
        {
            public ClaimsPrincipal User { get; set; }
        }

        public class GraphQLSettings
        {
            public Func<HttpContext, Task<object>> BuildUserContext { get; set; }
            public object Root { get; set; }
            public List<IValidationRule> ValidationRules { get; } = new List<IValidationRule>();
        }
    }
}