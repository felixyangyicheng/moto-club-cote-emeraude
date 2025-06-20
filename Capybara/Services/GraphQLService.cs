using Capybara.Models.Configs;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Polly;
using Polly.Timeout;
using System.Net;

public class GraphQLService : IDisposable
{
    private readonly ApiConfig _config;
    private readonly HttpClient _httpClient;
    private readonly AsyncTimeoutPolicy _timeoutPolicy;
    private readonly IList<GraphQLHttpClient> _clients = new List<GraphQLHttpClient>();

    public GraphQLService(ApiConfig config)
    {
        _config = config;
        _httpClient = new HttpClient { Timeout = Timeout.InfiniteTimeSpan };
        _timeoutPolicy = Policy.TimeoutAsync(TimeSpan.FromSeconds(config.TimeoutSeconds));
    }

    public async Task<GraphQLResponse<T>> SendQueryWithFallback<T>(GraphQLRequest request)
    {
        Exception lastException = null;

        foreach (var uri in _config.AllUris)
        {
            try
            {
                var client = CreateGraphQLClient(uri);
                return await _timeoutPolicy.ExecuteAsync(
                    async ct => await client.SendQueryAsync<T>(request, ct),
                    CancellationToken.None
                );
            }
            catch (Exception ex) when (IsRetryableException(ex))
            {
                lastException = ex;
                // Log: $"Échec sur {uri}, passage au suivant... Erreur: {ex.Message}"
            }
        }

        throw new AggregateException("Tous les endpoints ont échoué", lastException);
    }

    private GraphQLHttpClient CreateGraphQLClient(string uri)
    {
        var options = new GraphQLHttpClientOptions
        {
            EndPoint = new Uri(uri),
            HttpMessageHandler = new HttpClientHandler()
        };

        var client = new GraphQLHttpClient(options, new NewtonsoftJsonSerializer(), _httpClient);
        _clients.Add(client);
        return client;
    }

    private bool IsRetryableException(Exception ex)
    {
        return ex is TimeoutRejectedException   // Timeout Polly
            || ex is TimeoutException           // Timeout .NET
            || ex is HttpRequestException       // Erreurs réseau
            || (ex is GraphQLHttpRequestException gqlEx &&
                (int)gqlEx.StatusCode >= 500);   // Erreurs serveur (5xx)
    }

    public void Dispose()
    {
        foreach (var client in _clients)
        {
            client.Dispose();
        }
        _httpClient.Dispose();
    }
}