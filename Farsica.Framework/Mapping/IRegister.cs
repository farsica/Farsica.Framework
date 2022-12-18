namespace Farsica.Framework.Mapping
{
    using System.Diagnostics.CodeAnalysis;

    public interface IRegister
    {
        void Register([NotNull] TypeAdapterConfig config);
    }
}
