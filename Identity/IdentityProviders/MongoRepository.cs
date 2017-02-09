using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Business;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Identity.IdentityProviders
{
    public class MongoRepository : IDisposable
    {
        private readonly MongoCollection _collection;

        public MongoRepository()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var server = client.GetServer();

            var database = server.GetDatabase("foo");
            _collection = database.GetCollection<IdentityStorage>("bar");
        }

        public void Dispose()
        {
        }

#pragma warning disable 1998
        public async Task<IdentityUser> FindUser(string userName, string password)
#pragma warning restore 1998
        {
            // _collection.Insert(new IdentityUser());

            var query = new QueryBuilder<IdentityUser>();
            var queryAttributes = new List<IMongoQuery> { Query.EQ("UserName", userName), Query.EQ("Password", password) };

            var mongoQuery = query.And(queryAttributes);

            var list = _collection.FindAs<IdentityUser>(mongoQuery);
            var found = list.FirstOrDefault();

            if (found != null)
            {
                return new IdentityUser { UserName = found.UserName, Roles = found.Roles };
            }

            return default(IdentityUser);
        }
    }
}