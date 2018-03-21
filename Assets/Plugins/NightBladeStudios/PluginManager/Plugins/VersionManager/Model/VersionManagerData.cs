using System;
using UnityEngine;

namespace NightBladeStudios.PluginManager.VersionManager
{
    [Serializable]
    public class VersionManagerData : PluginData
    {
        [Header("Modules")] public bool playerSettingsVerifyModule;
        public bool updateRevisionOnRecompile;
        [Space] public string versionInfoPath = "Assets/VersionInfo.asset";
    }
}