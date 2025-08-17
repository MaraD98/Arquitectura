namespace Core.Application
{
    public interface IExternalApiClient
    {
        Task<string> GetAsync(string url, Tuple<string, string>? authentication = null, IDictionary<string, string>? headers = null);
        Task<HttpResponseMessage> PostAsync<T>(string uri, T item, Tuple<string, string>? authentication = null, IDictionary<string, string>? headers = null);
        Task<HttpResponseMessage> PutAsync<T>(string url, T item, Tuple<string, string>? authentication = null, IDictionary<string, string>? headers = null);
        Task<HttpResponseMessage> DeleteAsync(string url, Tuple<string, string>? authentication = null, IDictionary<string, string>? headers = null);
    }
}
