using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine.Networking;

static class EditorWWWHelper
{
    public static void Download(UnityWebRequest request, Action onComplete)
    {
        var operation = request.SendWebRequest();
        operation.completed += asyncOperation =>
        {
            onComplete.Invoke();
        };
    }

}
