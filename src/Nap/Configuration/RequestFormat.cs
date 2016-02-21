namespace Nap.Configuration
{
	/// <summary>
	/// A class containing the types of request formats commonly used in this application.
	/// </summary>
	public static class RequestFormat
    {
        /// <summary>
        /// The form content format, 'application/x-www-form-urlencoded'
        /// </summary>
        public const string Form = "application/x-www-form-urlencoded";

	    /// <summary>
	    /// The json content format, 'application/json'
	    /// </summary>
	    public const string Json = "application/json";

	    /// <summary>
	    /// The XML content format, 'application/xml'
	    /// </summary>
	    public const string Xml = "application/xml";

	    /// <summary>
	    /// The type corresponding to other, or unused, (de/)serialization.
	    /// </summary>
	    public const string Other = "OTHER";
    }
}