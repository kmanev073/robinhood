using System.Threading.Tasks;

namespace RobinHood
{
    public interface IHttpClient
    {
        Task<string> GetAsync(string uri, string authorizationToken = null, string authorizationMethod = "Bearer");

        Task<string> PostAsync<T>(string uri, T item, string authorizationToken = null, string authorizationMethod = "Bearer");
    }
}
