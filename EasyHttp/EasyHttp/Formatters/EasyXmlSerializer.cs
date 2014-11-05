using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using EasyHttp.Formatters.Base;

namespace EasyHttp.Formatters
{
    public class EasyXmlSerializer : IEasySerializer
    {
        public T Deserialize<T>(string serialized)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var sr = new StringReader(serialized))
            {
                return (T)xmlSerializer.Deserialize(sr);
            }
        }

        public string Serialize(object graph)
        {
            var xmlSerializer = new XmlSerializer(graph.GetType());
            using (var textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, graph);
                return textWriter.ToString();
            }
        }
    }
}
