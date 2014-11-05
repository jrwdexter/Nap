using System.Threading.Tasks;

namespace EasyHttp
{
    public interface IEasyHttpRequest
    {
        IEasyHttpRequest WithQueryParameter(string key, string value);
        IEasyHttpRequest WithBody(object body);
        IEasyHttpRequest WithHeader(string headerName, string value);
        Task<T> ExecuteAsync<T>();
        Task<string> ExecuteAsync();
    }
}