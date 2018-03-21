using UnityEngine;

namespace NightBladeStudios.PluginManager.Editor
{
    public class LocalPlugin : IPlugin
    {
        private const string Title = "Local Plugin";

        public string GetPluginTitle()
        {
            return Title;
        }

        public void GUI()
        {
            if (GUILayout.Button("Test Button"))
            {
            }

            if (GUILayout.Button("Example Button"))
            {
            }

            if (GUILayout.Button("Sample Button"))
            {
            }

            if (GUILayout.Button("Just Button"))
            {
            }

            if (GUILayout.Button("Super Button"))
            {
            }
        }
    }
}