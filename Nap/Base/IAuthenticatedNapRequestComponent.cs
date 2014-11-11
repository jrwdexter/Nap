namespace Nap
{
    public interface IAuthenticatedNapRequestComponent
    {
        IAdvancedNapRequestComponent Basic(string username, string password);
    }
}