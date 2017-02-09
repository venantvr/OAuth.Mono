using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Identity
{
    public class InMemoryRepository : IDisposable
    {
        private List<IdentityStorage> _storages = new List<IdentityStorage>();

        public InMemoryRepository()
        {
            _storages.Add (new IdentityStorage(){ 
                UserName = "test.test@mail.com", 
                Password = "test123", 
                Roles = new List<string>(){ "admins", "users" }});
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

        public void Dispose()
        {
        }
    }
}

