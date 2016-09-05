using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace AspNetCore.Identity.Marten
{
    public class IdentityUserLogin
    {
        // For Json Deserialization
        public IdentityUserLogin()
        {
        }

        public IdentityUserLogin(UserLoginInfo info)
        {
            if (info == null) throw new ArgumentNullException(nameof(info));

            LoginProvider = info.LoginProvider;
            ProviderKey = info.ProviderKey;
            ProviderDisplayName = info.ProviderDisplayName;
        }

        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }

        public string ProviderDisplayName { get; set; }

        public static implicit operator IdentityUserLogin(UserLoginInfo info) => new IdentityUserLogin(info);

        public static explicit operator UserLoginInfo(IdentityUserLogin identityLogin) => new UserLoginInfo(identityLogin.LoginProvider, identityLogin.ProviderKey, identityLogin.ProviderDisplayName);
    }
}
