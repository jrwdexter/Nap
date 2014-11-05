using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyHttp.Formatters.Base
{
    public interface IEasySerializer
    {
        T Deserialize<T>(string serialized);

        string Serialize(object graph);
    }
}