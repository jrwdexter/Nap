namespace Nap.Formatters.Base
{
    public interface INapSerializer
    {
        string ContentType { get; }

        T Deserialize<T>(string serialized);

        string Serialize(object graph);
    }
}