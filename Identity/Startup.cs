using System;
using System.Web.Http;
using Identity;
using Identity.IdentityProviders;
using Identity.Logging;
using Identity.OAuthProviders;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Owin.Security.AesDataProtectorProvider;
using Swashbuckle.Application;

[assembly: OwinStartup(typeof (Startup))]

namespace Identity
{
    public class Startup
    {
        public static string UserName => "test.test@mail.com";
        public static string Password => "test123";

        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            // Configure Web API for self-host. 
            var config = new HttpConfiguration();

            config.EnableCors();

            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional }
                );

            config
                .EnableSwagger(c => c.SingleApiVersion("v1", "Identity Server API"))
                .EnableSwaggerUi();

            config.MessageHandlers.Add(new ApiLogHandler());

            app.CreatePerOwinContext(() => new InMemoryRepository() as IUserRepository);

            // Web API configuration and services
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.Add(config.Formatters.JsonFormatter);

            OAuthOptions = new OAuthAuthorizationServerOptions
                           {
                               TokenEndpointPath = new PathString("/token"),
                               Provider = new CustomAuthorizationServerProvider(),
                               AllowInsecureHttp = true,
                               AccessTokenExpireTimeSpan = TimeSpan.FromDays(1)
                           };

            app.UseAesDataProtectorProvider();
            app.UseOAuthAuthorizationServer(OAuthOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            app.UseWebApi(config);
        }
    }
}