using IdentityServer.Providers;

namespace IdentityServer.Interfaces.Providers
{
    public interface IGoogleAuthProvider:IExternalAuthProvider
    {
        Provider Provider { get; }
    }
}
