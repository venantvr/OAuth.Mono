using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Identity
{
    public interface IUserRepository : IUserStore<IUser, string>, IDisposable
    {
        Task<IdentityUser> FindUser(string userName, string password);
    }
}

