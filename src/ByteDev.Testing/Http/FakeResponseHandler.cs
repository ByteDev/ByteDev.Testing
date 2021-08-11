using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ByteDev.Testing.Http
{
    public class FakeResponseHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode _httpStatusCode;
        private readonly HttpContent _httpContent;

        public FakeResponseHandler(HttpStatusCode httpStatusCode) : this(httpStatusCode, null)
        {
        }

        public FakeResponseHandler(HttpStatusCode httpStatusCode, HttpContent httpContent)
        {
            _httpStatusCode = httpStatusCode;
            _httpContent = httpContent;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(_httpStatusCode);

            if (_httpContent != null)
                response.Content = _httpContent;

            return Task.FromResult(response);
        }
    }
}