using System;
using System.Reflection;
using System.Text;
using Nap.Formatters.Base;

namespace Nap.Formatters
{
    public class NapFormsSerializer : INapSerializer
    {
        public T Deserialize<T>(string serialized)
        {
            throw new NotSupportedException("Forms deserialization is not supported.");
        }

        public string Serialize(object graph)
        {
            var sb = new StringBuilder();
            foreach (var prop in graph.GetType().GetRuntimeProperties())
            {
                sb.AppendFormat("{0}{1}&", prop.Name, prop.GetValue(graph));
            }

            return sb.ToString().TrimEnd('&');
        }
    }
}
