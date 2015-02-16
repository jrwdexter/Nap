namespace Nap.Configuration
{
    public enum RequestFormat
    {
        /// <summary>
        /// The form content format, 'application/x-www-form-urlencoded'
        /// </summary>
        Form,

        /// <summary>
        /// The json content format, 'application/json'
        /// </summary>
        Json,

        /// <summary>
        /// The XML content format, 'application/xml'
        /// </summary>
        Xml,

        /// <summary>
        /// The type corresponding to other, or unused, (de/)serialization.
        /// </summary>
        Other
    }
}