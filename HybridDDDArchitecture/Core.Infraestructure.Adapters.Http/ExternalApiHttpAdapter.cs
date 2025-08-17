using Core.Application;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Wrap;
using System.Net;
using System.Text;

namespace Core.Infraestructure
{
    public class ExternalApiHttpAdapter : IExternalApiClient
    {
        private readonly HttpClient _client;
        private readonly AsyncPolicyWrap<HttpResponseMessage> _policy;
        private readonly ILogger<ExternalApiHttpAdapter> _logger;

        public ExternalApiHttpAdapter(ILogger<ExternalApiHttpAdapter> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _client = new HttpClient();

            // Retry con espera exponencial
            AsyncRetryPolicy<HttpResponseMessage> retryPolicy = Policy
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .Or<HttpRequestException>()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                    onRetry: (outcome, timespan, attempt, context) =>
                    {
                        _logger.LogWarning($"[Retry] Intento {attempt} después de {timespan.TotalSeconds}s. Resultado: {outcome.Result?.StatusCode}");
                    });

            // Circuit Breaker
            AsyncCircuitBreakerPolicy<HttpResponseMessage> circuitBreakerPolicy = Policy
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .Or<HttpRequestException>()
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 5,
                    durationOfBreak: TimeSpan.FromSeconds(30),
                    onBreak: (outcome, timespan) =>
                    {
                        _logger.LogError($"[CircuitBreaker] Circuito ABIERTO durante {timespan.TotalSeconds}s. Último error: {outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString()}");
                    },
                    onReset: () => _logger.LogInformation("[CircuitBreaker] Circuito RESTABLECIDO."),
                    onHalfOpen: () => _logger.LogInformation("[CircuitBreaker] Circuito HALF-OPEN, probando..."));

            // Combinar Retry + CircuitBreaker
            _policy = Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);
        }

        public async Task<string> GetAsync(string url, Tuple<string, string>? authentication = null, IDictionary<string, string>? headers = null)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            AddAuthentication(requestMessage, authentication);
            AddHeaders(requestMessage, headers);

            HttpResponseMessage response = await _policy.ExecuteAsync(() => _client.SendAsync(requestMessage));

            return await GetResult(response);
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string uri, T item, Tuple<string, string>? authentication = null, IDictionary<string, string>? headers = null)
        {
            return await DoPostPutAsync(HttpMethod.Post, uri, item, authentication: authentication, headers: headers);
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string url, T item, Tuple<string, string>? authentication = null, IDictionary<string, string>? headers = null)
        {
            return await DoPostPutAsync(HttpMethod.Put, url, item, authentication: authentication, headers: headers);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url, Tuple<string, string>? authentication = null, IDictionary<string, string>? headers = null)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, url);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            AddAuthentication(requestMessage, authentication);
            AddHeaders(requestMessage, headers);

            HttpResponseMessage response = await _policy.ExecuteAsync(() => _client.SendAsync(requestMessage));

            return await CheckForErrors(response);
        }

        private async Task<HttpResponseMessage> DoPostPutAsync<T>(HttpMethod method, string url, T item, string contentType = "application/json", Tuple<string, string>? authentication = null, IDictionary<string, string>? headers = null)
        {
            if (method != HttpMethod.Post && method != HttpMethod.Put)
            {
                throw new ArgumentException("Value must be either post or put.", nameof(method));
            }

            HttpRequestMessage requestMessage = new HttpRequestMessage(method, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, contentType)
            };

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            AddAuthentication(requestMessage, authentication);
            AddHeaders(requestMessage, headers);

            HttpResponseMessage response = await _policy.ExecuteAsync(() => _client.SendAsync(requestMessage));

            return await CheckForErrors(response);
        }

        private async Task<HttpResponseMessage> CheckForErrors(HttpResponseMessage response)
        {
            string res = await response.Content.ReadAsStringAsync();

            switch (response.StatusCode)
            {
                case HttpStatusCode.InternalServerError:
                case HttpStatusCode.BadRequest:
                case HttpStatusCode.Unauthorized:
                    throw new HttpRequestException(res, null, response.StatusCode);

                default:
                    return response;
            }
        }

        private async Task<string> GetResult(HttpResponseMessage response)
        {
            return await response.Content.ReadAsStringAsync();
        }

        private void AddAuthentication(HttpRequestMessage requestMessage, Tuple<string, string>? authentication)
        {
            if (authentication == null) return;

            if (authentication.Item1.Equals("basic", StringComparison.OrdinalIgnoreCase))
            {
                var base64Auth = Convert.ToBase64String(Encoding.ASCII.GetBytes(authentication.Item2));
                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", base64Auth);
            }
            else if (authentication.Item1.Equals("bearer", StringComparison.OrdinalIgnoreCase))
            {
                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authentication.Item2);
            }
        }

        private void AddHeaders(HttpRequestMessage requestMessage, IDictionary<string, string>? headers)
        {
            if (headers == null) return;

            foreach (var header in headers)
            {
                requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }
    }
}
