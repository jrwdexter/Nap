using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

namespace EasyHttp
{
    public class EasyConfig
    {
        public Dictionary<RequestFormat, IEasySerializer> Serializers { get; set; }
        public string BaseUrl { get; set; }
        public string ContentType { get; set; }
        public RequestFormat AcceptFormat { get; set; }
        public RequestFormat ContentFormat { get; set; }
        public Uri Proxy { get; set; }

        public EasyConfig()
        {
            Serializers = new Dictionary<RequestFormat, IEasySerializer>();
        }
    }
}
