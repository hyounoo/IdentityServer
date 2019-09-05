using Newtonsoft.Json.Linq;

namespace IdentityServer.Interfaces.Providers
{
    public interface IExternalAuthProvider
    {
        JObject GetUserInfo(string accessToken);
    }
}
