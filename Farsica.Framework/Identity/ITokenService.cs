namespace Farsica.Framework.Identity
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Farsica.Framework.DataAnnotation;

    [Injectable]
    public interface ITokenService
    {
        Task<VerifyTokenResponse?> VerifyTokenAsync([NotNull] VerifyTokenRequest request);
    }
}
