using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Marten.Linq;

namespace AspNetCore.Identity.Marten.Internal
{
    internal class FindUsersByClaim<TUser, TKey> : ICompiledListQuery<TUser, TUser>
        where TUser : IdentityUser<TKey>
    {
        public IdentityUserClaim Claim { get; private set; }

        public FindUsersByClaim(IdentityUserClaim claim)
        {
            Claim = claim;
        }

        Expression<Func<IQueryable<TUser>, IEnumerable<TUser>>> ICompiledQuery<TUser, IEnumerable<TUser>>.QueryIs()
        {
            return q => q.Where(u => u.Claims.Any(c => c.Type == Claim.Type && c.Value == Claim.Value));
        }
    }
}