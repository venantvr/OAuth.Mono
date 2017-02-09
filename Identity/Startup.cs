using System;
using System.Security.Claims;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using OAuth20;
using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataHandler;
using Owin.Security.AesDataProtectorProvider;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Serializer;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.AspNet.Identity;
using System.Web.Http;
using Microsoft.Owin.Infrastructure;
using Newtonsoft.Json;
using Swashbuckle.Application;
using Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Identity;
using Microsoft.AspNet.Identity.Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace OAuth20
{
    public class Startup
    {
        public static string UserName => "test.test@mail.com";
        public static string Password => "test123";

        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();

            config.EnableCors();

            config.Routes.MapHttpRoute(
                name: "DefaultApi", 
                routeTemplate: "api/{controller}/{id}", 
                defaults: new { id = RouteParameter.Optional } 
            );

            config
                .EnableSwagger(c => c.SingleApiVersion("v1", "Identity Server API"))
                .EnableSwaggerUi();

            config.MessageHandlers.Add(new ApiLogHandler());

            app.CreatePerOwinContext<IUserRepository>(() => { return new InMemoryRepository() as IUserRepository; });

            // Web API configuration and services
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.Add(config.Formatters.JsonFormatter);

            OAuthOptions = new OAuthAuthorizationServerOptions()
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