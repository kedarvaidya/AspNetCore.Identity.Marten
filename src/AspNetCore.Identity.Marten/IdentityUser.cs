using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Identity.Marten
{
    public class IdentityUser<TKey>
    {
        public TKey Id { get; set; }

        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }
    }
}
