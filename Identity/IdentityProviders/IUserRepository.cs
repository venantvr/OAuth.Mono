using System.Threading.Tasks;
using Identity.Business;
using Microsoft.AspNet.Identity;

namespace Identity.IdentityProviders
{
    public interface IUserRepository : IUserStore<IUser, string>
    {
        Task<IdentityUser> FindUser(string userName, string password);
    }
}