namespace Nap
{
    internal partial class NapRequest : IAuthenticatedNapRequestComponent
    {
        public IAdvancedNapRequestComponent Basic(string username, string password)
        {
            return this;
        }
    }
}
