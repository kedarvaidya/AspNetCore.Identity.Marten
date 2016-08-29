using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AspNetCore.Identity.Marten
{
    public static class IdentityMartenBuilderExtensions
    {
        /// <summary>
        /// Adds a Marten implementation of identity information stores.
        /// </summary>
        /// <typeparam name="TUser"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="self">The <see cref="IdentityBuilder"/> instance this method extends.</param>
        /// <returns>The <see cref="IdentityBuilder"/> instance this method extends.</returns>
        public static IdentityBuilder AddMartenStores<TUser, TKey>(this IdentityBuilder self) where TUser: IdentityUser<TKey>
        {
            self.Services.TryAddScoped<IUserStore<TUser>, UserStore<TUser, TKey>>();

            return self;
        }
    }
}
