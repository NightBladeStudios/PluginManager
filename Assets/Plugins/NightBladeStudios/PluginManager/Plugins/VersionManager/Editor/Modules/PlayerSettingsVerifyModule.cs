using NightBladeStudios.PluginManager.VersionManager;
using UnityEditor;

namespace NightBladeStudios.PluginManager.Editor.VersionManager.Modules
{
    public static class PlayerSettingsVerifyModule
    {
        public static void Invoke()
        {
            ShowCheckingDialog();
        }

        public static void ShowCheckingDialog()
        {
            if (PlayerSettings.companyName == "DefaultCompany")
                EditorUtility.DisplayDialog("Default Company Name!", "You should change Company Name", "Ok");
            if (PlayerSettings.applicationIdentifier == "com.Company.ProductName")
                EditorUtility.DisplayDialog("Default Identifier!", "You should change Bundle Identifier", "Ok");
        }

        public static bool IsActive()
        {
            return PluginDataHelper.GetData<VersionManagerData>().playerSettingsVerifyModule;
        }


        public static void Activate(bool state)
        {
            PluginDataHelper.GetData<VersionManagerData>().playerSettingsVerifyModule = state;
        }
    }
}