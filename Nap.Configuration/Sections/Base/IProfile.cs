namespace Nap.Configuration.Sections.Base
{
    /// <summary>
    /// Describes a profile for a nap configuration; that is, a nap configuration with a string key.
    /// </summary>
    public interface IProfile
    {
        /// <summary>
        /// Gets or sets the key of the profile.  Often the website name.
        /// </summary>
        /// <example>"google" for https://www.google.com</example>
        string Key { get; set; }

        /// <summary>
        /// Gets or sets the nap configuration that belongs to this profile.
        /// </summary>
        ChildNapConfig Value { get; }
    }
}