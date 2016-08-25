using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Marten.Linq;

namespace AspNetCore.Identity.Marten.Internal
{
    internal class FindUserByNormalizedUserName<TUser, TKey> : ICompiledQuery<TUser, TUser>
        where TUser: IdentityUser<TKey>
    {
        public string NormalizedUserName { get; private set; }

        public FindUserByNormalizedUserName(string normalizedUserName)
        {
            NormalizedUserName = normalizedUserName;
        }

        public Expression<Func<IQueryable<TUser>, TUser>> QueryIs()
        {
            return q => q.SingleOrDefault(u => u.NormalizedUserName == NormalizedUserName);
        }
    }
}
