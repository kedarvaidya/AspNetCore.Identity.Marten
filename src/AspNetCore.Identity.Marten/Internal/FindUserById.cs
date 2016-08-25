using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Marten.Linq;

namespace AspNetCore.Identity.Marten.Internal
{
    internal class FindUserById<TUser, TKey> : ICompiledQuery<TUser, TUser>
        where TUser : IdentityUser<TKey>
    {
        public string Id { get; private set; }

        public FindUserById(string id)
        {
            Id = id;
        }

        public Expression<Func<IQueryable<TUser>, TUser>> QueryIs()
        {
            return q => q.SingleOrDefault(u => u.HasStringifiedId(Id));
        }
    }
}
