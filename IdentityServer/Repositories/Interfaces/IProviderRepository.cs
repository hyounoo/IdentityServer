using IdentityServer.Providers;
using System.Collections.Generic;

namespace IdentityServer.Repositories.Interfaces
{
    public interface IProviderRepository
    {
        IEnumerable<Provider> Get();
    }
}
