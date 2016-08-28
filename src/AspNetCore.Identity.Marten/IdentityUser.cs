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

        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public DateTimeOffset? EmailConfirmedAt { get; set; }

        public string PhoneNumber { get; set; }
        public DateTimeOffset? PhoneNumberConfirmedAt { get; set; }

        public DateTimeOffset? TwoFactorEnabledAt { get; set; }

        public DateTimeOffset? LockoutEnabledAt { get; set; }
        public DateTimeOffset? LockoutEndsAt { get; set; }
        public IList<DateTimeOffset> AccessFailedAt { get; set; } = new List<DateTimeOffset>();
    }
}
