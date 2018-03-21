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
    private const string RepoURL =
        "https://raw.githubusercontent.com/NightBladeStudios/PluginManager/master/Repository.json?token=ABwhJlcBl4dfyHPtqpQ-H036Z7v4gCd_ks5au8FbwA%3D%3D";

    private static PluginRepository repo;

    #region Reflection

    public static void LoadPlugins()
    {
        plugins = new List<IPlugin>();
        foreach (Type mytype in System.Reflection.Assembly.GetExecutingAssembly().GetTypes())
        {
            if (mytype.GetInterfaces().Contains(typeof(IPlugin)))
            {
                plugins.Add((IPlugin)Activator.CreateInstance(mytype));
            }
        }

        foldouts = Enumerable.Repeat(false, plugins.Count).ToList();
    }


    #endregion

    #region Window

    private const string WindowTitle = "Plugin Manager";

    private static List<IPlugin> plugins;
    private static List<bool> foldouts;


    [PreferenceItem(WindowTitle)]

    public static void GUI()
    {

        if (plugins == null)
        {
            LoadPlugins();
        }

        if (repo == null)
        {
            LoadPluginRepo();
        }

        if (GUILayout.Button("Update"))
        {
            LoadPluginRepo();

        }

        if (repo != null)
        {
            DrawOfficialPlugins();
            DrawLocalPlugins();
        }
        else
        {
            GUILayout.Label("Loading Repository...");

        }
    }

    private static void DrawLocalPlugins()
    {
        for (int index = 0; index < plugins.Count; index++)
        {
            if (!CheckIfOfficial(plugins[index].GetPluginTitle()))
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                DrawPlugin(index);

                EditorGUILayout.EndVertical();
            }

        }
    }

    private static void DrawOfficialPlugins()
    {
        if (repo.Plugins != null)
            for (int index = 0; index < repo.Plugins.Count; index++)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                if (CheckIfDownloaded(index))
                {

                    DrawPlugin(GetPluginIndex(repo.Plugins[index].Name));

                }
                else
                {

                    DrawAvailableOfficialPlugin(index);

                }

                EditorGUILayout.EndVertical();
            }
    }

    private static bool CheckIfOfficial(string name)
    {
        return repo != null && (repo.Plugins != null && repo.Plugins.Exists(plugin => plugin.Name == name));
    }

    private static bool CheckIfDownloaded(int index)
    {
        return plugins.Exists(plugin => plugin.GetPluginTitle() == repo.Plugins[index].Name);
    }

    private static void DrawAvailableOfficialPlugin(int index)
    {
        if (EditorGUILayout.ToggleLeft(repo.Plugins[index].Name, false))
        {
            DownloadAsset(repo.Plugins[index].URL);

        }
    }

    private static int GetPluginIndex(string name)
    {
        return plugins.FindIndex(plugin => plugin.GetPluginTitle() == name);
    }

    private static void DrawPlugin(int index)
    {
        foldouts[index] = EditorGUILayout.Foldout(foldouts[index],
            plugins[index].GetPluginTitle());

        if (foldouts[index])
        {
            plugins[index].GUI();
        }
    }
    #endregion

    #region Download Methods


    public static void LoadPluginRepo()
    {
        UnityWebRequest uwr = UnityWebRequest.Get(RepoURL);
        EditorWWWHelper.Download(uwr, () =>
        {
            var json = uwr.downloadHandler.text;
            repo = JsonUtility.FromJson<PluginRepository>(json);
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
