using System;
using System.Collections.Generic;

namespace Identity
{
    public class IdentityStorage
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public List<string> Roles { get; set; }
    }
}

