using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNet.Identity;

namespace Identity
{
    public class InMemoryRepository : IDisposable, IUserRepository
    {
        private List<IdentityStorage> _storages = new List<IdentityStorage>();

        public InMemoryRepository()
        {
            _storages.Add(new IdentityStorage()
                { 
                    UserName = "test.test@mail.com", 
                    Password = "test123", 
                    Roles = new List<string>(){ "admins", "users" }
                });
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            var found = _storages.FirstOrDefault(s => s.UserName == userName && s.Password == password);

            if (found != null)
            {
                return new IdentityUser(){ UserName = found.UserName, Roles = found.Roles };                
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

        public Task<IUser> FindByIdAsync(/*TKey*/ string userId)
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

