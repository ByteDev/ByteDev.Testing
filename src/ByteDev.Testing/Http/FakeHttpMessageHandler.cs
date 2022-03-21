using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ByteDev.Testing.Http
{
    /// <summary>
    /// Represents a fake HttpMessageHandler for returning a corresponding response or
    /// exception thrown upon each HTTP request.
    /// </summary>
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        private int _index;
        private readonly IList<FakeRequestOutcome> _outcomes;

        /// <summary>
        /// Indicates if the provided outcomes should be repeated when reaching the end of
        /// the outcome sequence. False by default.
        /// </summary>
        public bool RepeatOutcomes { get; set; }

        /// <summary>
        /// Number of requests actually made.
        /// </summary>
        public int RequestsMade { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Testing.Http.FakeHttpMessageHandler" /> class.
        /// </summary>
        /// <param name="outcome">Outcome to perform upon a single HTTP request.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="outcome" /> is null.</exception>
        public FakeHttpMessageHandler(FakeRequestOutcome outcome)
        {
            if (outcome == null)
                throw new ArgumentNullException(nameof(outcome));

            _outcomes = new List<FakeRequestOutcome> { outcome };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Testing.Http.FakeHttpMessageHandler" /> class.
        /// </summary>
        /// <param name="outcomes">Sequence of outcomes to perform each upon each corresponding HTTP request.</param>
        /// <exception cref="T:System.ArgumentException"><paramref name="outcomes" /> is null or empty.</exception>
        public FakeHttpMessageHandler(IList<FakeRequestOutcome> outcomes)
        {
            if (outcomes == null || outcomes.Count == 0)
                throw new ArgumentException("Outcomes was either null or empty.");

            _outcomes = outcomes;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var outcome = GetRequestOutcome();

            _index++;
            RequestsMade++;

            if (outcome.HasException)
            {
                throw outcome.Exception;
            }

            return Task.FromResult(outcome.HttpResponseMessage);
        }

        private FakeRequestOutcome GetRequestOutcome()
        {
            try
            {
                if (_index >= _outcomes.Count && RepeatOutcomes)
                    _index = 0;

                return _outcomes[_index];
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException($"Request number {RequestsMade + 1} has no corresponding outcome.", ex);
            }
        }
    }
}