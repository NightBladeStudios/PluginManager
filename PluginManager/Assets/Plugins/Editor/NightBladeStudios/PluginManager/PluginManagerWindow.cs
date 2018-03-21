using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class PluginManagerWindow : Editor
{
    private const string RepoURL = "file://C:/Source/PluginManager/PluginManager/Assets/PluginRepo.json";
    private static PluginRepository Repo;
    
    #region Reflection
    public static void LoadPlugins()
    {
        _plugins = new List<IPlugin>();
        foreach (Type mytype in System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
            .Where(mytype => mytype.GetInterfaces().Contains(typeof(IPlugin))))
        {
            _plugins.Add((IPlugin)Activator.CreateInstance(mytype));
        }
        _foldouts = Enumerable.Repeat(false, _plugins.Count).ToList();
    }


    #endregion

    #region Window
    private const string WindowTitle = "Plugin Manager";
    
    private static List<IPlugin> _plugins;
    private static List<bool> _foldouts;
    

    [PreferenceItem(WindowTitle)]

    public static void GUI()
    {
       
        if (_plugins == null)
        {
            LoadPlugins();
        }

        if (Repo == null)
        {
            LoadPluginRepo();
        }
        
        if (GUILayout.Button("Update"))
        {
            LoadPluginRepo();

        }

        if (Repo != null)
            if (Repo.Plugins != null)
                for (int index = 0; index < Repo.Plugins.Count; index++)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                    if (_plugins.Exists(plugin => plugin.GetPluginTitle() == Repo.Plugins[index].Name))
                    {
                        _foldouts[index] = EditorGUILayout.Foldout(_foldouts[index],
                            _plugins[index].GetPluginTitle());

                        if (_foldouts[index])
                        {
                            _plugins[index].GUI();
                        }
                    }
                    else
                    {
                        if (EditorGUILayout.ToggleLeft(Repo.Plugins[index].Name, false))
                        {
                            DownloadAsset(Repo.Plugins[index].URL);

                        }
                    }

                    EditorGUILayout.EndVertical();

                }

        //        for (var index = 0; index < window._plugins.Count; index++)
        //        {
        //            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        //
        //            EditorGUILayout.EndVertical();
        //        }

        //        {
        //                ADBPath = EditorPrefs.GetString("AndroidADBPath", "");
        //                BuildLocation = EditorPrefs.GetString("AndroidBuildLocation", "");
        //                ReleaseVersion = EditorPrefs.GetString("AndroidBuildReleaseVersion", "Alpha");
        //                LogcatFilters =
        //                    JsonUtility.FromJson<LogcatFilterWrapper>(EditorPrefs.GetString("LogcatFilters",
        //                        JsonUtility.ToJson(new LogcatFilterWrapper())));
        //
        //                prefsLoaded = true;
        //        }

        //        if (GUILayout.Button("Add Filter"))
        //        {
        //                LogcatFilters.Filters.Add(new LogcatFilter {Color = Color.black, Contains = "Unity"});
        //                GUI.changed = true;
        //                Debug.Log(JsonUtility.ToJson(LogcatFilters));
        //        }


        //            if (!GUI.changed) return;
        //
        //            EditorPrefs.SetString("AndroidADBPath", ADBPath);
        //            EditorPrefs.SetString("AndroidBuildLocation", BuildLocation);
        //            EditorPrefs.SetString("AndroidBuildReleaseVersion", ReleaseVersion);
        //            EditorPrefs.SetBool("AndroidConfigured", ADBPath != "" && BuildLocation != "" && ReleaseVersion != "");
        //            EditorPrefs.SetString("LogcatFilters", JsonUtility.ToJson(LogcatFilters));
    }
    #endregion

    #region Download Methods

    
    public static void LoadPluginRepo()
    {
        UnityWebRequest uwr = UnityWebRequest.Get(RepoURL);
        EditorWWWHelper.Download(uwr, () =>
        {
            var json = uwr.downloadHandler.text;
            Repo = JsonUtility.FromJson<PluginRepository>(json);
            uwr.Dispose();
        });
    }

    public static void DownloadAsset(string url)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(url);
        EditorWWWHelper.Download(uwr, () =>
        {
            var packageFolderName = "UnityPluginManager";
            var projectRoot = Path.GetPathRoot(Application.dataPath);
            var downloadPath = Path.Combine(projectRoot, packageFolderName);
            if (!Directory.Exists(downloadPath)) Directory.CreateDirectory(downloadPath);
            var path = Path.Combine(downloadPath, Path.GetFileName(url));
            File.WriteAllBytes(path, uwr.downloadHandler.data);
            AssetDatabase.ImportPackage(path, false);
            uwr.Dispose();

        });
    }

    #endregion
}
