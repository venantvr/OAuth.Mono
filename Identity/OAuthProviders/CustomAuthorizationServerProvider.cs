using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Identity.Business;
using Identity.IdentityProviders;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace Identity.OAuthProviders
{
    public class CustomAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public IdentityUser Identity { get; set; }

#pragma warning disable 1998
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
#pragma warning restore 1998
        {
            var oAuthIdentity = new ClaimsIdentity(context.Options.AuthenticationType);

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
                var assemblyQualifiedName = typeof (IUserRepository).AssemblyQualifiedName;

                if (assemblyQualifiedName != null)
                {
                    using (var repository = context.OwinContext.Get<IUserRepository>($"AspNet.Identity.Owin:{assemblyQualifiedName}"))
                    {
                        var user = await repository.FindUser(clientId, clientSecret);

                        if (user == null)
                        {
                            context.SetError("invalid_grant", "The user name or password is incorrect");

                            return;
                        }

                        Identity = user;
                    }
                }
            }

            context.Validated(clientId);
        }
    }
}