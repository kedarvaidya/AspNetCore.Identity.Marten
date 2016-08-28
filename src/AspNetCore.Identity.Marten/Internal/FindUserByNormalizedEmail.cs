using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Marten.Linq;

namespace AspNetCore.Identity.Marten.Internal
{
    internal class FindUserByNormalizedEmail<TUser, TKey> : ICompiledQuery<TUser, TUser>
        where TUser : IdentityUser<TKey>
    {
        public string NormalizedEmail { get; private set; }

        public FindUserByNormalizedEmail(string normalizedEmail)
        {
            NormalizedEmail = normalizedEmail;
        }

        public Expression<Func<IQueryable<TUser>, TUser>> QueryIs()
        {
            return q => q.SingleOrDefault(u => u.NormalizedEmail == NormalizedEmail);
        }
    }
}