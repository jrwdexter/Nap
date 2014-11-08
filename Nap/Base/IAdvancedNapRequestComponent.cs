using System;
using System.Collections.Generic;
using System.Linq;

namespace Napper
{
    public interface IAdvancedNapRequestComponent
    {
        IAdvancedNapRequestComponent UseProxy(Uri uri);

        IAuthenticatedNapRequestComponent Authentication { get; } 
    }
}
