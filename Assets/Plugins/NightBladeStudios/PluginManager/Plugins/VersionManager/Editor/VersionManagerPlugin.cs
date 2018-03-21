using NightBladeStudios.PluginManager.VersionManager;
using UnityEditor;

namespace NightBladeStudios.PluginManager.Editor.VersionManager
{
    public class VersionManagerPlugin : IPlugin
    {
        public const string Title = "Version Manager";

        public static VersionManagerData data;

        public string GetPluginTitle()
        {
            return Title;
        }

        public void GUI()
        {
            if (data == null) data = PluginDataHelper.GetData<VersionManagerData>();
            var versionInfo = (VersionInfo) AssetDatabase.LoadAssetAtPath(data.versionInfoPath, typeof(VersionInfo));
            if (versionInfo == null) versionInfo = VersionInfoHelper.Create(data.versionInfoPath);
            data.DrawGUI();
        }
    }
}