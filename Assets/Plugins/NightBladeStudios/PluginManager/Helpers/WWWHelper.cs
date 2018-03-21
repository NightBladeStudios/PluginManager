using System;
using UnityEngine.Networking;

namespace NightBladeStudios.PluginManager
{
    public static class WWWHelper
    {
        public static void Download(this UnityWebRequest request, Action onComplete)
        {
            var operation = request.SendWebRequest();
            operation.completed += asyncOperation =>
            {
                onComplete.Invoke();
                request.Dispose();
            };
        }
    }
}