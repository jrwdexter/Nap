using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

using Nap.Exceptions;
using Nap.Formatters.Base;

namespace Nap.Formatters
{
    /// <summary>
    /// The formatter corresponding to the "application/xml" MIME type.
    /// Utilizes <see cref="XmlSerializer"/> to perform serialization/deserialization.
    /// </summary>
    public class NapXmlFormatter : INapFormatter
    {
        /// <summary>
        /// Gets the MIME type corresponding to a given implementation of the <see cref="INapFormatter"/> interface.
        /// Returns "application/xml".
        /// </summary>
        public string ContentType => "application/xml";

        /// <summary>
        /// Converts a serialized value to a C# object.  A new object is created in this process.
        /// </summary>
        /// <typeparam name="T">The type of object that should be created from the serializedvalue.</typeparam>
        /// <param name="serialized">The serialized data; such as from a REST request.</param>
        /// <returns>The newly created object of type <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="serialized"/> is null.</exception>
        /// <exception cref="ConstructorNotFoundException">Thrown when a paremeterless constructor is not found for type <typeparamref name="T"/>.</exception>
        /// <remarks>All properties that are being hydrated by deserialization must have public setters.</remarks>
        /// <example><code>
        /// &lt;?xml version="1.0" encoding="utf-16"?&gt;
        /// &lt;Person xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema"&gt;
        ///    &lt;FirstName&gt;John&lt;/FirstName&gt;
        ///    &lt;LastName&gt;Doe&lt;/LastName&gt;
        /// &lt;/Person&gt;
        /// </code>
        /// would populate an object like
        /// <code>
        /// public class Person
        /// {
        ///     public string FirstName { get; set; }
        ///     public string LastName { get; set; }
        /// }
        /// </code>
        /// </example>
        public T Deserialize<T>(string serialized)
        {
            if(serialized == null)
                throw new ArgumentNullException(nameof(serialized), "Cannot deserialize null.");
            if (typeof(T).GetTypeInfo().DeclaredConstructors.All(c => c.ContainsGenericParameters || c.GetParameters().Any()))
                throw new ConstructorNotFoundException("Paremeterless constructor was not fuond.");

            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var sr = new StringReader(serialized))
            {
                return (T)xmlSerializer.Deserialize(sr);
            }
        }

        /// <summary>
        /// Converts an object to a simple string to be transported via some protocol.
        /// Uses <see cref="Newtonsoft.Json.JsonConvert" />.
        /// </summary>
        /// <param name="graph">The object to serialize.</param>
        /// <returns>The object graph serialized to a string.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="graph"/> is null.</exception>
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
        /// &lt;?xml version="1.0" encoding="utf-16"?&gt;
        /// &lt;Person xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema"&gt;
        ///    &lt;FirstName&gt;John&lt;/FirstName&gt;
        ///    &lt;LastName&gt;Doe&lt;/LastName&gt;
        /// &lt;/Person&gt;
        /// </code>
        /// </example>
        public string Serialize(object graph)
        {
            if(graph == null)
                throw new ArgumentNullException(nameof(graph), "Cannot serialize a null object graph.");

            var xmlSerializer = new XmlSerializer(graph.GetType());
            using (var textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, graph);
                return textWriter.ToString();
            }
        }
    }
}
