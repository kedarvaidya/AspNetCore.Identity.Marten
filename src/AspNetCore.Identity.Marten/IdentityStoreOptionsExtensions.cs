using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Identity.Marten.Internal;
using Marten;
using Microsoft.AspNetCore.Builder;

namespace AspNetCore.Identity.Marten
{
    public static class IdentityStoreOptionsExtensions
    {
        public static void ConfigureIdentityStoreOptions<TUser, TKey>(this StoreOptions self, IdentityOptions identityOptions) where TUser: IdentityUser<TKey>
        {
            self.Linq.MethodCallParsers.Add(new HasStringifiedId<TKey>());

			var userSchema = self.Schema.For<TUser>();
			userSchema.Index(u => u.NormalizedUserName, i => { i.IsUnique = true; });
			if(identityOptions.User.RequireUniqueEmail)
				userSchema.Index(u => u.NormalizedEmail, i => { i.IsUnique = true; });
		}
	}
}
