using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace Napper
{
    public interface IAuthenticatedNapRequestComponent
    {
        IAdvancedNapRequestComponent Basic(string username, SecureString password);
    }
}