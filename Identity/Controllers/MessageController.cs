using System;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using Identity.Business;

namespace Identity.Controllers
{
    [EnableCors("*", "*", "*")]
    [Authorize(Roles = "admins")]
    public class MessageController : ApiController
    {
        // GET api/values
        public Message Get()
        {
            return new Message
                   {
                       Now = DateTime.Now,
                       WhoAmI = Dns.GetHostEntry(Dns.GetHostName()).HostName,
                       Content = @"Nice to see you !"
                   };
        }

        // GET api/values/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/values
        public void Post([FromBody] Message value)
        {
        }
    }
}