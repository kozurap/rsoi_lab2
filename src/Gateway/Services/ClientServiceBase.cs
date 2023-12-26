using Gateway.Wrappers;

namespace Gateway.Services
{
    public abstract class ClientServiceBase
    {
        protected abstract string BaseUri { get; }
        protected readonly HttpClientDecorator Client;

        public ClientServiceBase()
        {
            Client = new HttpClientDecorator();
        }

        private string GetBaseUri() => BaseUri;

        protected string BuildUri(string stringToAppend)
        {
            var baseUri = GetBaseUri();
            baseUri = baseUri.Last() != '/' ? baseUri + '/' : baseUri;
            return baseUri + stringToAppend;
        }
    }
}
