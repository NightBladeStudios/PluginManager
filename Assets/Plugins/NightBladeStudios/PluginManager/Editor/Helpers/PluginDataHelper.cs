using UnityEditor;
using UnityEngine;

namespace NightBladeStudios.PluginManager.Editor
{
    public static class PluginDataHelper
    {
        private static UnityEditor.Editor _editor;

        public static T GetData<T>() where T : PluginData
        {
            var data = (T) EditorGUIUtility.Load(typeof(T).Name + ".asset");
            if (data == null) data = Create<T>();
            return data;
        }

        public static T Create<T>() where T : PluginData
        {
            var data = (T) ScriptableObject.CreateInstance(typeof(T));
            AssetDatabase.CreateAsset(data, string.Format("Assets/Editor Default Resources/{0}.asset", typeof(T).Name));
            AssetDatabase.SaveAssets();
            return data;
        }

        public static void DrawGUI(this PluginData data)
        {
            if (_editor == null)
                _editor = UnityEditor.Editor.CreateEditor(data);
            _editor.DrawDefaultInspector();
        }
    }
}