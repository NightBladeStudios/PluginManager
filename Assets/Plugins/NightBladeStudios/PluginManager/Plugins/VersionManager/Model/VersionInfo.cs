using System;
using UnityEngine;

namespace NightBladeStudios.PluginManager.VersionManager
{
    public class VersionInfo : ScriptableObject
    {
        public long creationTime;
        public string flags;
        public bool isDevelopement;
        public BuildVersion version;


        public string GetVersion()
        {
            return version.ToString();
        }

        public string GetFlags()
        {
            return isDevelopement ? "-dev" + flags : flags;
        }

        public string GetCompilationDateTime()
        {
            return DateTime.FromFileTime(creationTime).AddDays(version.build).AddSeconds(version.revison * 2)
                .ToString("dd MMMM yyyy - HH:mm:ss ");
        }

        public string GetCompilationDate()
        {
            return DateTime.FromFileTime(creationTime).AddDays(version.build).ToString("dd MMMM yyyy");
        }

        public string GetCompilationTime()
        {
            return DateTime.FromFileTime(creationTime).AddSeconds(version.revison * 2).ToString("T");
        }
    }
}