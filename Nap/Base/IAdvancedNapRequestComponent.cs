using System;

namespace Nap
{
    public interface IAdvancedNapRequestComponent
    {
        IAdvancedNapRequestComponent UseProxy(Uri uri);

        IAuthenticatedNapRequestComponent Authentication { get; } 
    }
}
