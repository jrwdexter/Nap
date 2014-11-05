using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace EasyHttp
{
    public class EasyHttpRequest : IEasyHttpRequest
    {
        private EasyHttpClient _client;
        public string Url { get; set; }
        public RequestFormat AcceptFormat { get; set; }
        public RequestFormat ContentFormat { get; set; }
        public HttpMethod Method { get; set; }
        public readonly List<KeyValuePair<string, string>> QueryParameters = new List<KeyValuePair<string, string>>();
        public readonly List<KeyValuePair<string, string>> Headers = new List<KeyValuePair<string, string>>();
        public string Body { get; set; }

        internal EasyHttpRequest(EasyHttpClient client)
        {
            _client = client;
        }

        public IEasyHttpRequest WithQueryParameter(string key, string value)
        {
            this.QueryParameters.Add(new KeyValuePair<string, string>(key, value));
            return this;
        }
        public IEasyHttpRequest WithBody(object body)
        {
            Body = _client.Config.Serializers[ContentFormat].Serialize(body);
            return this;
        }

        public IEasyHttpRequest WithHeader(string headerName, string value)
        {
            this.Headers.Add(new KeyValuePair<string, string>(headerName, value));
            return this;
        }
        public async Task<string> ExecuteAsync()
        {
            var handler = new HttpClientHandler();
            if (_client.Config.Proxy != null)
            {
                handler.Proxy = new WebProxy(_client.Config.Proxy);
                handler.UseProxy = true;
            }
            var client = new HttpClient(handler);
            var content = new StringContent(this.Body);
            string urlTemp = this.Url;
            if (this.QueryParameters.Any())
            {
                urlTemp = this.Url.Contains("?") ?
                    (this.Url.EndsWith("?") ? this.Url : this.Url + "&") :
                    this.Url + "?";
                urlTemp += string.Join("&", this.QueryParameters.Select(x => string.Format("{0}={1}", x.Key, WebUtility.UrlEncode(x.Value))));
            }
            foreach (var header in Headers)
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            HttpResponseMessage response = null;
            if (this.Method == HttpMethod.Get)
                response = await client.GetAsync(new Uri(urlTemp));
            if (this.Method == HttpMethod.Post)
                response = await client.PostAsync(new Uri(urlTemp), content);
            if (this.Method == HttpMethod.Put)
                response = await client.PutAsync(new Uri(urlTemp), content);
            if (this.Method == HttpMethod.Delete)
                response = await client.DeleteAsync(new Uri(urlTemp));
            if (response == null)
                return default(string);

            using (var ms = new MemoryStream())
            using (var sr = new StreamReader(ms))
            {
                await response.Content.CopyToAsync(ms);
                ms.Position = 0;
                return await sr.ReadToEndAsync();

            }
        }

        public async Task<T> ExecuteAsync<T>()
        {
            return _client.Config.Serializers[AcceptFormat].Deserialize<T>(await ExecuteAsync());
        }

    }
}