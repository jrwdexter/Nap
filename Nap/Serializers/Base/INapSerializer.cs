namespace Nap.Serializers.Base
{
    /// <summary>
    /// The interface that needs to be implemented for any sort of serialization technique.
    /// Serializers in Nap can be used on both request and response objects seperately.
    /// (for example, a request could be serialized using JSON, while a response could be deserialized using XML).
    /// </summary>
    public interface INapSerializer
    {
        /// <summary>
        /// Gets the MIME type corresponding to a given implementation of the <see cref="INapSerializer"/> interface.
        /// </summary>
        /// <example>A <see cref="NapJsonSerializer"/> would return "application/json".</example>
        string ContentType { get; }

        /// <summary>
        /// Converts a serialized value to a C# object.  A new object is created in this process.
        /// </summary>
        /// <typeparam name="T">The type of object that should be created from the serializedvalue.</typeparam>
        /// <param name="serialized">The serialized data; such as from a REST request.</param>
        /// <returns>The newly created object of type <typeparamref name="T"/>.</returns>
        /// <remarks>All properties that are being hydrated by deserialization must have public setters.</remarks>
        /// <example><code>
        /// {
        ///     "FirstName" : "John",
        ///     "LastName" : "Doe"
        /// }
        /// </code>
        /// would populate an object like
        /// <code>
        /// public class Person
        /// {
        ///     public string FirstName { get; set; }
        ///     public string LastName { get; set; }
        /// }
        /// </code>
        /// for JSON deserialization.
        /// </example>
        T Deserialize<T>(string serialized);

        /// <summary>
        /// Converts an object to a simple string to be transported via some protocol.
        /// </summary>
        /// <param name="graph">The object to serialize.</param>
        /// <returns>The object graph serialized to a string.</returns>
        /// <remarks>All properties that are being serialized must have public getters.</remarks>
        /// <example><code>
        /// public class Person
        /// {
        ///     public string FirstName { get; set; }
        ///     public string LastName { get; set; }
        /// }
        /// </code>
        /// would be serialized as
        /// <code>
        /// {
        ///     "FirstName" : "John",
        ///     "LastName" : "Doe"
        /// }
        /// </code>
        /// for JSON serialization.
        /// </example>
        string Serialize(object graph);
    }
}