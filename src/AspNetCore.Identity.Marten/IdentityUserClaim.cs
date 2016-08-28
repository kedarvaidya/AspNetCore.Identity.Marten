using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNetCore.Identity.Marten
{
    public class IdentityUserClaim
    {
        public IdentityUserClaim(Claim claim)
        {
            if (claim == null) throw new ArgumentNullException(nameof(claim));

            this.Type = claim.Type;
            this.Value = claim.Value;
        }

        public string Type { get; private set; }

        public string Value { get; private set; }

        public static implicit operator IdentityUserClaim(Claim claim) => new IdentityUserClaim(claim);

        public static explicit operator Claim(IdentityUserClaim identityClaim) => new Claim(identityClaim.Type, identityClaim.Value);

        internal void RebuildFrom(Claim claim)
        {
            if (claim == null) throw new ArgumentNullException(nameof(claim));

            this.Type = claim.Type;
            this.Value = claim.Value;
        }
    }
}
