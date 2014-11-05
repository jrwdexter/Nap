using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EasyHttp
{
    public class EasyHttpClient
    {
        public EasyHttpRequest Get(string url, RequestFormat format = RequestFormat.Json)
        {
            return new EasyHttpRequest { Url = url, Format = format, Method = HttpMethod.Get };
        }
        public EasyHttpRequest Post(string url, RequestFormat format = RequestFormat.Json)
        {
            return new EasyHttpRequest { Url = url, Format = format, Method = HttpMethod.Post };
        }

        public EasyHttpRequest Delete(string url, RequestFormat format = RequestFormat.Json)
        {
            return new EasyHttpRequest { Url = url, Format = format, Method = HttpMethod.Delete };
        }

        public EasyHttpRequest Put(string url, RequestFormat format = RequestFormat.Json)
        {
            return new EasyHttpRequest { Url = url, Format = format, Method = HttpMethod.Put };
        }
    }
}
