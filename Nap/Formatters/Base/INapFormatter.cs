namespace Nap.Formatters.Base
{
    public interface INapSerializer
    {
        T Deserialize<T>(string serialized);

        string Serialize(object graph);
    }
}