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

            // Web API configuration and services
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.Add(config.Formatters.JsonFormatter);

            OAuthOptions = new OAuthAuthorizationServerOptions()
            {
                TokenEndpointPath = new PathString("/token"),
                Provider = new OAuthAuthorizationServerProvider()
                {
                    OnValidateClientAuthentication = async (context) =>
                    {
                        context.Validated();
                    },
                    OnGrantResourceOwnerCredentials = async (context) =>
                    {
                        if (context.UserName == UserName && context.Password == Password)
                        {
                            ClaimsIdentity oAuthIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
                            context.Validated(oAuthIdentity);
                        }
                    }
                },
                AllowInsecureHttp = true,
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1)
            };

            app.UseAesDataProtectorProvider();
            app.UseOAuthAuthorizationServer(OAuthOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());  
            app.UseWebApi(config); 

            // app.UseOAuthBearerTokens(OAuthOptions);
        }
    }

    public class Token
    {
        [JsonProperty("access_token")]  
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]  
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]  
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]  
        public string RefreshToken { get; set; }
    }
}