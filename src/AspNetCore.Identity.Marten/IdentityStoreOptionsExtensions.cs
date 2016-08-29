using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Identity.Marten.Internal;
using Marten;

namespace AspNetCore.Identity.Marten
{
    public static class IdentityStoreOptionsExtensions
    {
        public static void ConfigureIdentityStoreOptions<TUser, TKey>(this StoreOptions self) where TUser: IdentityUser<TKey>
        {
            self.Linq.MethodCallParsers.Add(new HasStringifiedId<TKey>());

            self.Schema.For<TUser>().Index(u => u.NormalizedUserName, i =>
            {
                i.IsUnique = true;
            });

            self.Schema.For<TUser>().Index(u => u.NormalizedEmail, i =>
            {
                i.IsUnique = true;
            });
        }
    }
}
