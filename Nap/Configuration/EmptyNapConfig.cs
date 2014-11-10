using System.Collections.Generic;
using Napper.Formatters.Base;

namespace Napper.Configuration
{
    public class EmptyNapConfig : INapConfig
    {
        public Dictionary<RequestFormat, INapSerializer> Serializers { get; set; }
        public string BaseUrl { get; set; }
        public RequestFormat Serialization { get; set; }
        public IAdvancedNapConfig Advanced { get; set; }
        public bool FillMetadata { get; set; }

        public INapConfig Clone()
        {
            return this;
        }
    }
}