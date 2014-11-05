using System;
using System.Collections.Generic;
using System.Linq;

using Napper.Formatters.Base;

using Newtonsoft.Json;

namespace Napper.Formatters
{
    public class NapJsonSerializer : INapSerializer
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
