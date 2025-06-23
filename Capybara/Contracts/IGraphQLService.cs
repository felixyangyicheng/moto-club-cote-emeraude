namespace Capybara.Contracts
{
    public interface IGraphQLService
    {
        Task<GraphQLResponse<T>> SendQueryWithFallback<T>(GraphQLRequest request);
        void Dispose();
    }
}
