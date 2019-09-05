using IdentityServer.Providers;

namespace IdentityServer.Interfaces.Providers
{
    public interface IGitHubAuthProvider : IExternalAuthProvider
    {
        Provider Provider { get; }
    }
}
