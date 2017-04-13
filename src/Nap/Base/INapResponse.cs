using System.Collections.Generic;

namespace Nap
{
    public interface INapResponse
    {
        string Url { get; set; }
        INapRequest Request { get; set; }
        string Content { get; set; }

        IReadOnlyDictionary<string, IReadOnlyCollection<string>> Headers { get; set; }
        IReadOnlyDictionary<INapCookie>
        IReadOnlyCollection<string, string> Cookies { get; set; }
        //IEnumerable<>
    }
}