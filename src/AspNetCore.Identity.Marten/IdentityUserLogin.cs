using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AspNetCore.Identity.Marten
{
    public class IdentityUserLogin
    {
        public IdentityUserLogin(UserLoginInfo info)
        {
            LoginProvider = info.LoginProvider;
            ProviderKey = info.ProviderKey;
            ProviderDisplayName = info.ProviderDisplayName;
        }

        public string LoginProvider { get; private set; }

        public string ProviderKey { get; private set; }

        public string ProviderDisplayName { get; private set; }

        public static implicit operator IdentityUserLogin(UserLoginInfo info) => new IdentityUserLogin(info);

        public static explicit operator UserLoginInfo(IdentityUserLogin identityLogin) => new UserLoginInfo(identityLogin.LoginProvider, identityLogin.ProviderKey, identityLogin.ProviderDisplayName);
    }
}
