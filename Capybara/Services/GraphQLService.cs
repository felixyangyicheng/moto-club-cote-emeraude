using Capybara.Models.Configs;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Timeout;
using System.Net;

using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Timeout;
using System.Net;

public class GraphQLService : IGraphQLService, IDisposable
{
    private readonly ApiConfig _config;
    private readonly HttpClient _httpClient;
    private readonly IAsyncPolicy _resiliencePolicy;

    public GraphQLService(
        IOptions<ApiConfig> config,
        IHttpClientFactory httpClientFactory)
    {
        _config = config.Value;
        _config.Validate(); // Double validation

        _httpClient = httpClientFactory.CreateClient();
        _httpClient.Timeout = Timeout.InfiniteTimeSpan;

        // 1. Politique de Timeout
        var timeoutPolicy = Policy.TimeoutAsync(
            TimeSpan.FromSeconds(_config.TimeoutSeconds),
            TimeoutStrategy.Optimistic);

        // 2. Politique de Retry avec basculement d'URL
        var retryPolicy = Policy
            .Handle<Exception>(IsRetryableException)
            .RetryAsync(_config.AllUris.Count - 1, (ex, retryCount, context) =>
            {
                context["currentUriIndex"] = retryCount;
            });

        // Pipeline Polly (Retry > Timeout)
        _resiliencePolicy = Policy.WrapAsync(retryPolicy, timeoutPolicy);
    }

    public async Task<GraphQLResponse<T>> SendQueryWithFallback<T>(GraphQLRequest request)
    {
        var context = new Context
        {
            ["currentUriIndex"] = 0  // Start with primary URL
        };

        return await _resiliencePolicy.ExecuteAsync(async ctx =>
        {
            var index = (int)ctx["currentUriIndex"];
            var uri = _config.AllUris[index];

            using var client = new GraphQLHttpClient(
                new GraphQLHttpClientOptions { EndPoint = new Uri(uri) },
                new NewtonsoftJsonSerializer(),
                _httpClient);

            return await client.SendQueryAsync<T>(request);
        }, context);
    }

    private bool IsRetryableException(Exception ex)
    {
        return ex is TimeoutRejectedException   // Polly timeout
            || ex is TimeoutException           // System timeout
            || ex is HttpRequestException       // Network errors
            || (ex is GraphQLHttpRequestException gqlEx &&
                (int?)gqlEx.StatusCode >= 500); // Server errors (5xx)
    }

    public void Dispose() => _httpClient?.Dispose();
}