using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Identity.Marten
{
    public class IdentityUserAuthenticationToken
    {
        // For Json Deserialization
        public IdentityUserAuthenticationToken()
        {
        }

        public IdentityUserAuthenticationToken(string loginProvider, string name, string value)
        {
            if (String.IsNullOrWhiteSpace(loginProvider)) throw new ArgumentNullException(nameof(loginProvider));
            if (String.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (String.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));

            LoginProvider = loginProvider;
            Name = name;
            Value = value;
        }

        public virtual string LoginProvider { get; set; }

        public virtual string Name { get; set; }

        public virtual string Value { get; set; }
    }
}
