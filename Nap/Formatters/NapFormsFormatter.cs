using System;
using System.Reflection;
using System.Text;

using Nap.Formatters.Base;

namespace Nap.Formatters
{
    /// <summary>
    /// The formatter corresponding to the "application/x-www-form-urlencoded" MIME type.
    /// </summary>
    /// <remarks>Does not support the <see cref="Deserialize{T}"/> method.</remarks>
    public class NapFormsFormatter : INapFormatter
    {
        /// <summary>
        /// Gets the MIME type corresponding to a given implementation of the <see cref="INapFormatter"/> interface.
        /// Returns "application/x-www-form-urlencoded".
        /// </summary>
        public string ContentType => "application/x-www-form-urlencoded";

        /// <summary>
        /// Not implemented for Forms serialization.
        /// </summary>
        /// <typeparam name="T">Unused - not implemented for Forms serializaiton.</typeparam>
        /// <param name="serialized">Unused - not implemented for Forms serializaiton.</param>
        /// <returns>Not implemented for forms serialization.</returns>
        public T Deserialize<T>(string serialized)
        {
            throw new NotSupportedException("Forms deserialization is not supported.");
        }

        /// <summary>
        /// Converts an object to a simple string to be transported via forms serialization.
        /// </summary>
        /// <param name="graph">The object to serialize.</param>
        /// <returns>The object graph serialized to a string.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <see cref="graph"/> is null.</exception>
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
        /// FirstName=John&LastName=Doe
        /// </code>
        /// for Forms serialization.
        /// </example>
        public string Serialize(object graph)
        {
            if(graph == null)
                throw new ArgumentNullException(nameof(graph), "Cannot serialize a null object graph.");

            var sb = new StringBuilder();
            foreach (var prop in graph.GetType().GetRuntimeProperties())
            {
                sb.AppendFormat("{0}={1}&", prop.Name, prop.GetValue(graph));
            }

            return sb.ToString().TrimEnd('&');
        }
    }
}
