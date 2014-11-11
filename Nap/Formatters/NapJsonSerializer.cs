using Nap.Formatters.Base;
using Newtonsoft.Json;

namespace Nap.Formatters
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
