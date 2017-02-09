using System;
using Microsoft.Owin.Security.OAuth;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.Owin.Security;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace Identity
{
    public class CustomAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public IdentityUser Identity { get; set; }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            ClaimsIdentity oAuthIdentity = new ClaimsIdentity(context.Options.AuthenticationType);

            var properties = new AuthenticationProperties(new Dictionary<string, string>
                {
                    { "dm:appid", @"user.Id.ToString()" },
                    { "dm:apikey", @"Convert.ToBase64String(secretKeyBytes)" }
                });

            foreach (var role in Identity.Roles)
            {
                oAuthIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            var ticket = new AuthenticationTicket(oAuthIdentity, properties);

            context.Validated(ticket);
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId;
            string clientSecret;

            if (context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                using (var repository = context.OwinContext.Get<IUserRepository>("AspNet.Identity.Owin:" + (typeof(IUserRepository)).AssemblyQualifiedName.ToString()))
                {
                    IdentityUser user = await repository.FindUser(clientId, clientSecret);

                    if (user == null)
                    {
                        context.SetError("invalid_grant", "The user name or password is incorrect");

                        return;
                    }

                    Identity = user;
                }
            }

            context.Validated(clientId);
        }
    }
}

