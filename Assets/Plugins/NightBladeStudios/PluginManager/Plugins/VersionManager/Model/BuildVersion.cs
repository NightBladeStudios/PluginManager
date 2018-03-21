using System;

namespace NightBladeStudios.PluginManager.VersionManager
{
    [Serializable]
    public class BuildVersion
    {
        public int build;
        public int major;
        public int minor;
        public int revison;

        public BuildVersion(string bundleVersion)
        {
            var versionArray = bundleVersion.Split('.');
            major = versionArray.Length >= 1 ? int.Parse(versionArray[0]) : 0;
            minor = versionArray.Length >= 2 ? int.Parse(versionArray[1]) : 0;
            build = versionArray.Length >= 3 ? int.Parse(versionArray[2]) : 0;
            revison = versionArray.Length >= 4 ? int.Parse(versionArray[3]) : 0;
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}.{2}.{3}", major, minor, build, revison);
        }

        protected bool Equals(BuildVersion other)
        {
            return major == other.major && minor == other.minor && build == other.build && revison == other.revison;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = major;
                hashCode = (hashCode * 397) ^ minor;
                hashCode = (hashCode * 397) ^ build;
                hashCode = (hashCode * 397) ^ revison;
                return hashCode;
            }
        }

        public BuildVersion Copy()
        {
            return new BuildVersion(ToString());
        }

        public BuildVersion IncreaseRevision()
        {
            revison++;
            return this;
        }

        public BuildVersion SetTimeVersion(DateTime creationTime)
        {
            SetTimeBuild(creationTime);
            SetTimeRevision();
            return this;
        }

        public BuildVersion SetTimeRevision()
        {
            revison = (int) DateTime.Now.TimeOfDay.TotalSeconds / 2;
            return this;
        }

        public BuildVersion SetTimeBuild(DateTime creationTime)
        {
            build = DateTime.Today.Subtract(creationTime).Days;
            return this;
        }

        public BuildVersion IncreaseBuild()
        {
            build++;
            revison = 0;
            return this;
        }

        public BuildVersion IncreaseMinor()
        {
            minor++;
            build = 0;
            revison = 0;
            return this;
        }

        public BuildVersion IncreaseMajor()
        {
            major++;
            minor = 0;
            build = 0;
            revison = 0;
            return this;
        }
    }
}