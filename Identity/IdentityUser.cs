using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace Identity
{
    public class IdentityUser : IUser<string>
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public List<string> Roles { get; set; }
    }
}

