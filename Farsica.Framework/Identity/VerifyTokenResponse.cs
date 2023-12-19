namespace Farsica.Framework.Identity
{
    using System.Collections.Generic;
    using System.Security.Claims;

    public class VerifyTokenResponse
    {
        public IEnumerable<Claim>? Claims { get; set; }
    }
}
