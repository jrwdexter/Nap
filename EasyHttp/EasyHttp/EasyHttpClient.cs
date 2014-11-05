using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

using EasyHttp.Formatters;

namespace EasyHttp
{
    public class EasyHttpClient : IDisposable
    {
        public EasyConfig Config { get; set; }

        public EasyHttpClient()
        {
            Config = new EasyConfig();
            Config.Serializers.Add(RequestFormat.Json, new EasyJsonSerializer());
            Config.Serializers.Add(RequestFormat.Xml, new EasyXmlSerializer());
        }

        public IEasyHttpRequest Get(string url, RequestFormat format = RequestFormat.Json)
        {
            return new EasyHttpRequest(this) { Url = url, ContentFormat = format, Method = HttpMethod.Get };
        }
        public IEasyHttpRequest Post(string url, RequestFormat format = RequestFormat.Json)
        {
            return new EasyHttpRequest(this) { Url = url, ContentFormat = format, Method = HttpMethod.Post };
        }

        public IEasyHttpRequest Delete(string url, RequestFormat format = RequestFormat.Json)
        {
            return new EasyHttpRequest(this) { Url = url, ContentFormat = format, Method = HttpMethod.Delete };
        }

        public IEasyHttpRequest Put(string url, RequestFormat format = RequestFormat.Json)
        {
            return new EasyHttpRequest(this) { Url = url, ContentFormat = format, Method = HttpMethod.Put };
        }

        public void Dispose()
        {
        }
    }
}
