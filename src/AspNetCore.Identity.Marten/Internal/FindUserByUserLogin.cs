using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Marten.Linq;

namespace AspNetCore.Identity.Marten.Internal
{
    internal class FindByLogin<TUser, TKey> : ICompiledQuery<TUser, TUser>
        where TUser : IdentityUser<TKey>
    {
        public string LoginProvider { get; private set; }
        public string ProviderKey { get; private set; }

        public FindByLogin(string loginProvider, string providerKey)
        {
            LoginProvider = loginProvider;
            ProviderKey = providerKey;
        }

        public Expression<Func<IQueryable<TUser>, TUser>> QueryIs()
        {
            return q => q.SingleOrDefault(u => u.Logins.Any(l => l.LoginProvider == LoginProvider && l.ProviderKey == ProviderKey));
        }
    }
}