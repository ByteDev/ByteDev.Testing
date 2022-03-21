using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ByteDev.Testing.Http
{
    /// <summary>
    /// Represents the outcome (response or exception thrown) for a HTTP request.
    /// </summary>
    public class FakeRequestOutcome
    {
        internal bool HasException => Exception != null;

        /// <summary>
        /// Exception that will be thrown for a request.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Response to return for a request.
        /// </summary>
        public HttpResponseMessage HttpResponseMessage { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Testing.Http.FakeRequestOutcome" /> class.
        /// </summary>
        /// <param name="exception">Exception that will be thrown for a request.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="exception" /> is null.</exception>
        public FakeRequestOutcome(Exception exception)
        {
            Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Testing.Http.FakeRequestOutcome" /> class.
        /// </summary>
        /// <param name="httpResponseMessage">Response to return for a request.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="httpResponseMessage" /> is null.</exception>
        public FakeRequestOutcome(HttpResponseMessage httpResponseMessage)
        {
            HttpResponseMessage = httpResponseMessage ?? throw new ArgumentNullException(nameof(httpResponseMessage));
        }

        /// <summary>
        /// Create a outcome that represents a request timeout from the server.
        /// </summary>
        /// <returns>New outcome instance.</returns>
        public static FakeRequestOutcome CreateRequestTimeout()
        {
            // Before .NET5 operation timeouts cause a TaskCanceledException (with no wrapped TimeoutException)
            return new FakeRequestOutcome(new TaskCanceledException("Request has timed out."));
        }

        /// <summary>
        /// Create a outcome that represents no known host.
        /// </summary>
        /// <returns>New outcome instance.</returns>
        public static FakeRequestOutcome CreateNoSuchHost()
        {
            return new FakeRequestOutcome(new HttpRequestException("No such host is known."));
        }
    }
}