using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Business;
using Microsoft.AspNet.Identity;

namespace Identity.IdentityProviders
{
    public class InMemoryRepository : IUserRepository
    {
        private readonly List<IdentityStorage> _storages = new List<IdentityStorage>();

        public InMemoryRepository()
        {
            _storages.Add(new IdentityStorage
                          {
                              UserName = "test.test@mail.com",
                              Password = "test123",
                              Roles = new List<string> { "admins", "users" }
                          });
        }

#pragma warning disable 1998
        public async Task<IdentityUser> FindUser(string userName, string password)
#pragma warning restore 1998
        {
            var found = _storages.FirstOrDefault(s => s.UserName == userName && s.Password == password);

            if (found != null)
            {
                return new IdentityUser { UserName = found.UserName, Roles = found.Roles };
            }

            return default(IdentityUser);
        }

        public Task CreateAsync(IUser user)
        {
            return null;
        }

        public Task DeleteAsync(IUser user)
        {
            return null;
        }

        public Task<IUser> FindByIdAsync(string userId)
        {
            return null;
        }

        public Task<IUser> FindByNameAsync(string userName)
        {
            return null;
        }

        public Task UpdateAsync(IUser user)
        {
            return null;
        }

        public void Dispose()
        {
        }
    }
}