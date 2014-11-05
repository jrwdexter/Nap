using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EasyHttp.Formatters
{
    public class EasyFormsSerializer : IEasySerializer
    {
        public T Deserialize<T>(string serialized)
        {
            throw new NotSupportedException("Forms deserialization is not supported.");
        }

        public string Serialize(object graph)
        {
            var sb = new StringBuilder();
            foreach (var prop in graph.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                sb.AppendFormat("{0}{1}&", prop.Name, prop.GetValue(graph));
            }
            return sb.ToString().TrimEnd('&');
        }
    }
}
