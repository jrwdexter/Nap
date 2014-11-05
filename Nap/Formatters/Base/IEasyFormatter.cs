using System;
using System.Collections.Generic;
using System.Linq;

namespace Napper.Formatters.Base
{
    public interface INapSerializer
    {
        T Deserialize<T>(string serialized);

        string Serialize(object graph);
    }
}