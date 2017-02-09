using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace Identity.Business
{
    public class IdentityUser : IUser<string>
    {
        public List<string> Roles { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
    }
}