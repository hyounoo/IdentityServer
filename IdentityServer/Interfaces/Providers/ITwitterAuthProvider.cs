using IdentityServer.Providers;

namespace IdentityServer.Interfaces.Providers
{
    public interface ITwitterAuthProvider : IExternalAuthProvider
    {
        Provider Provider { get; }
    }
}
