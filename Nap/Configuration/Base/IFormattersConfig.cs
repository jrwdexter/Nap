using System;
using System.Collections.Generic;
using System.Linq;

using Nap.Formatters.Base;

namespace Nap.Configuration
{
	/// <summary>
	/// Represents a collection of formatters that can have additional formatters added.
	/// </summary>
	public interface IFormattersConfig : ICollection<IFormatterConfig>
	{
		/// <summary>
		/// Adds the specified formatter by specifying key/value pair.
		/// </summary>
		/// <param name="contentType">The key of the formatter to add (see MIME types).</param>
		/// <param name="formatterType">The full type name of the formatter to add.</param>
		void Add(string contentType, string formatterType);

        /// <summary>
        /// Adds the specified formatter generically.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="INapFormatter"/> to add.</typeparam>
        void Add<T>() where T : INapFormatter, new();

        /// <summary>
        /// Adds the specified formatter generically.
        /// </summary>
        /// <param name="contentType">The content type to apply the formatter to.</param>
        /// <typeparam name="T">The type of <see cref="INapFormatter"/> to add.</typeparam>
        void Add<T>(string contentType) where T : INapFormatter, new();

        /// <summary>
        /// Adds the specified formatter by specifying an instance of the formatter.
        /// </summary>
        /// <param name="napFormatter">The formatter instance to add to the collection of formatters.</param>
        void Add(INapFormatter napFormatter);

        /// <summary>
        /// Adds the specified formatter by specifying an instance of the formatter.
        /// </summary>
        /// <param name="napFormatter">The formatter instance to add to the collection of formatters.</param>
        /// <param name="contentType">The content type to apply the formatter to.</param>
        void Add(INapFormatter napFormatter, string contentType);

        /// <summary>
        /// Remvoes the specified formatter by key, a string of appropriate MIME type.
        /// </summary>
        /// <param name="contentType">The MIME type key of the formatter to remove.</param>
        void Remove(string contentType);

		/// <summary>
		/// Converts the <see cref="IFormattersConfig"/> interface to a dicitonary.
		/// Note that operations on this object (such as <see cref="IDictionary{T1,T2}.Add(T1, T2)"/>) do not persist.
		/// </summary>
		/// <returns>The <see cref="IFormattersConfig"/> interface as a dictionary.</returns>
		IDictionary<string, INapFormatter> AsDictionary();
	}
}