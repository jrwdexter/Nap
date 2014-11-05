using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

namespace EasyHttp.Formatters
{
    public class EasyJsonSerializer : IEasySerializer
    {
        public T Deserialize<T>(string serialized)
        {
            return JsonConvert.DeserializeObject<T>(serialized);
        }

        public string Serialize(object graph)
        {
            return JsonConvert.SerializeObject(graph);
        }
    }
}
