using System;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity
{
    public class MongoRepository : IDisposable
    {
        private MongoCollection _collection;

        public MongoRepository ()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var server = client.GetServer();

            var database = server.GetDatabase("foo");
            _collection = database.GetCollection<IdentityStorage>("bar");
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            // _collection.Insert(new IdentityUser());

            var query = new QueryBuilder<IdentityUser>();
            var queryAttributes = new List<IMongoQuery>();

            queryAttributes.Add(Query.EQ("UserName", userName));
            queryAttributes.Add(Query.EQ("Password", password));

            IMongoQuery mongoQuery = query.And(queryAttributes);

            var list = _collection.FindAs<IdentityUser>(mongoQuery);
            var found = list.FirstOrDefault();

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

