using Nap.Configuration.Sections.Base;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Configuration;

namespace Nap.Configuration.Sections
{
    /// <summary>
    /// Describes a collection of profiles that can be used to initialize certain configurations.
    /// </summary>
    public class Profiles : NapConfigurationCollectionBase<Profile, IProfile, ChildNapConfig>, IProfiles
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Profiles"/> class.
        /// </summary>
        public Profiles() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Profiles"/> class.
        /// </summary>
        /// <param name="profiles">The profiles used to initialize the profiles collection.</param>
        public Profiles(IEnumerable<IProfile> profiles)
        {
            foreach (var profile in profiles)
                Add(new Profile { Key = profile.Key, Value = (ChildNapConfig)profile.Value.Clone() });
        }
    }
}