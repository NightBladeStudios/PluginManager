using System;
using NightBladeStudios.PluginManager.VersionManager;
using UnityEditor;
using UnityEngine;

namespace NightBladeStudios.PluginManager.Editor.VersionManager
{
    public static class VersionInfoHelper
    {
        public static VersionInfo Create(string path)
        {
            var versionInfo = (VersionInfo) ScriptableObject.CreateInstance(typeof(VersionInfo));
            versionInfo.creationTime = DateTime.Today.ToFileTime();
            versionInfo.version = new BuildVersion("0.0.0.0");
            AssetDatabase.CreateAsset(versionInfo, path);
            AssetDatabase.SaveAssets();
            return versionInfo;
        }
    }
}