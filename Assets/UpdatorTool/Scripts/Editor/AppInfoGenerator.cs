using System;
using UnityEditor;
using UnityEngine;
using UpdatorTool.Scripts.Runtime;

namespace UpdatorTool.Scripts.Editor
{
    public class AppInfoGenerator : EditorWindow
    {
        private AppInfo info;
        
        [MenuItem("App Updator/AppInfo Generator")]
        private static void OpenAppInfoGenerator()
        {
            var win = EditorWindow.GetWindow<AppInfoGenerator>("AppInfo Generator");
            win.ShowUtility();
        }

        private void OnEnable()
        {
            if (info == null)
                info = new AppInfo();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical("groupbox");
            info.AppName = EditorGUILayout.TextField("App Name:", info.AppName);
            info.Author = EditorGUILayout.TextField("Author:", info.Author);
            info.AppVersion = EditorGUILayout.TextField("App Version:", info.AppVersion);
            info.BuildVersion = EditorGUILayout.TextField("Build Version:", info.BuildVersion);
            info.MandatoryUpdate = EditorGUILayout.Toggle("Mandatory Update:", info.MandatoryUpdate);
            info.DevBuild = EditorGUILayout.Toggle("Development Build:", info.DevBuild);
            EditorGUILayout.EndVertical();
            if (GUILayout.Button("Generate AppInfo"))
            {
                var path = EditorUtility.SaveFilePanel("Save AppInfo", "", "AppInfo", Constants.APPINFO_FILE_EXT);
                if (string.IsNullOrEmpty(path)) return;
                AppInfoFileIO.WriteAppInfoFile(info, path);
            }
        }
    }
}