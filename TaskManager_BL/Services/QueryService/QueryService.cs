namespace TaskTracker_BL.Services.QueryService

{
    public class QueryService : IQueryService
    {
        public Dictionary<string, string> CreateQueryParams(string email, string emailKey)
        {
            Dictionary<string, string> queryParams = new Dictionary<string, string>();
            queryParams.Add("email", email);
            queryParams.Add("key", emailKey);
            return queryParams;
        }

        public void AddQueryParams(UriBuilder uriBuilder, Dictionary<string, string> queryParams)
        {
            uriBuilder.Query = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"));
        }
    }
}
