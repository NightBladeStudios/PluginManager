using System;
using NightBladeStudios.PluginManager.Editor.VersionManager.Modules;
using NightBladeStudios.PluginManager.VersionManager;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Callbacks;

namespace NightBladeStudios.PluginManager.Editor.VersionManager
{
    public class BuildPreProcessor : IPreprocessBuild
    {
        public int callbackOrder { get; private set; }

        public void OnPreprocessBuild(BuildTarget target, string path)
        {
            var pluginData = PluginDataHelper.GetData<VersionManagerData>();
            var versionInfo =
                (VersionInfo) AssetDatabase.LoadAssetAtPath(pluginData.versionInfoPath, typeof(VersionInfo));

            PlayerSettings.bundleVersion = versionInfo.version.ToString();

            if (PlayerSettingsVerifyModule.IsActive()) PlayerSettingsVerifyModule.Invoke();
        }


        [DidReloadScripts]
        public static void OnRecompile()
        {
            var data = PluginDataHelper.GetData<VersionManagerData>();
            if (!data.updateRevisionOnRecompile) return;

            var versionInfo = (VersionInfo) AssetDatabase.LoadAssetAtPath(data.versionInfoPath, typeof(VersionInfo));
            versionInfo.version.SetTimeVersion(DateTime.FromFileTime(versionInfo.creationTime));
        }
    }
}