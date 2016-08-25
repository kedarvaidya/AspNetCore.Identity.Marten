using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Identity.Marten.Internal
{
    internal static class IdentityUserExtensions
    {
        public static bool HasStringifiedId<TKey>(this IdentityUser<TKey> self, string id)
        {
            return self.Id.ToString() == id;
        }
    }
}
