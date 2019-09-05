using IdentityServer.Providers;

namespace IdentityServer.Interfaces.Providers
{
    public interface IFacebookAuthProvider : IExternalAuthProvider
    {
        Provider Provider { get; }
    }
}
