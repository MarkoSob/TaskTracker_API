namespace TaskTracker_BL.Services.QueryService
{
    public interface IQueryService
    {
        Dictionary<string, string> CreateQueryParams(string email, string emailKey);
        void AddQueryParams(UriBuilder uriBuilder, Dictionary<string, string> queryParams);
    }
}
