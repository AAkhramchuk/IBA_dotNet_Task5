using IdentityServer4.Validation;
using IdentityServer4.Models;

namespace IdentityServer
{
    // allows arbitrary redirect URIs - only for demo purposes. NEVER USE IN PRODUCTION
    public class DemoRedirectValidator : IRedirectUriValidator
    {
        public Task<bool> IsPostLogoutRedirectUriValidAsync(string requestedUri, Client client)
        {
            return Task.FromResult(true);
        }

        public Task<bool> IsRedirectUriValidAsync(string requestedUri, Client client)
        {
            return Task.FromResult(true);
        }
    }
}
