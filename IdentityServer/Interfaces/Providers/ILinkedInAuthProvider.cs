using IdentityServer.Providers;

namespace IdentityServer.Interfaces.Providers
{
    public interface ILinkedInAuthProvider : IExternalAuthProvider
    {
        Provider Provider { get; }
    }
}
