using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UpdatorTool.Scripts.Runtime;
using System.IO;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression;

public class UpdateGenerator : EditorWindow
{
    private AppInfo info;
    private bool BuildInProgress { get; set; }
    private float BuildProgress { get; set; }

    private string BuildPath => $"{AppUpdatorSettings.Settings.AppBuildPath}/{info.FileName}";
    
    [MenuItem("App Updator/Create New Update")]
    private static void OpenUpdateGenerationWindow()
    {
        var win = EditorWindow.GetWindow<UpdateGenerator>("App Update Generator",true);
        win.ShowUtility();
    }

    private void OnEnable()
    {
        if (info == null)
            info = AppUpdatorSettings.Settings.CurrentVersion;
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical("groupbox");
        EditorGUI.BeginChangeCheck();
        GUILayout.Space(10);
        EditorGUILayout.HelpBox("Set your new app info settings.", MessageType.Info);
        GUILayout.Space(20);
        info.AppName = EditorGUILayout.TextField("App Name:", PlayerSettings.productName);
        info.Author = EditorGUILayout.TextField("Author:", PlayerSettings.companyName);
        info.AppVersion = EditorGUILayout.TextField("App Version:", info.AppVersion);
        info.BuildVersion = EditorGUILayout.TextField("Build Version:", info.BuildVersion);
        info.MandatoryUpdate = EditorGUILayout.Toggle("Mandatory Update:", info.MandatoryUpdate);
        info.DevBuild = EditorGUILayout.Toggle("Development Build:", info.DevBuild);
        EditorGUILayout.EndVertical();
        if (string.IsNullOrEmpty(AppUpdatorSettings.Settings.AppBuildPath))
        {
            if(GUILayout.Button("Choose Build Path"))
            {
                AppUpdatorSettings.Settings.AppBuildPath =
                    EditorUtility.SaveFolderPanel($"Choose Location for {info.AppName}", "", "");
            }
        }

        if (EditorGUI.EndChangeCheck())
        {
           EditorUtility.SetDirty(AppUpdatorSettings.Settings);
           AssetDatabase.SaveAssets();
        }
        
        GUILayout.Space(10);
        var ogColor = GUI.color;
        GUI.color = Color.cyan;
        EditorGUILayout.LabelField("Build will be generated at");
        GUI.color = Color.cyan;
        EditorGUILayout.LabelField(BuildPath);
        GUILayout.Space(10);
        
        if (!BuildInProgress && !string.IsNullOrEmpty(AppUpdatorSettings.Settings.AppBuildPath))
        {
            GUI.color = Color.green;
            ShowBuildButton();
        }
        
        GUILayout.FlexibleSpace();
        
        if (BuildInProgress)
        {
            GUI.color = Color.cyan;
            ShowBuildProgress();
        }

        GUI.color = ogColor;
        GUILayout.Space(10);
    }

    private void ShowBuildProgress()
    {
        var progressRect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(20));
        EditorGUI.ProgressBar(progressRect,BuildProgress, "Update Generation Progress");
    }

    private void ShowBuildButton()
    {
        if (GUILayout.Button("Create Update File"))
        {
            MakeBuild();
        }
    }

    private void MakeBuild()
    {
        BuildInProgress = true;
        info.BuildTime = DateTime.UtcNow.ToString();
        EditorUtility.SetDirty(AppUpdatorSettings.Settings);
        AssetDatabase.SaveAssets();
        
        string[] levels = new string[EditorSceneManager.sceneCount];
        for (int i = 0; i < EditorSceneManager.sceneCount; i++)
        {
            levels[i] = EditorSceneManager.GetSceneAt(i).path;
        }

        if (Directory.Exists(BuildPath))
            Directory.CreateDirectory(BuildPath);
        
        BuildProgress = 0f / 3;
        // Build player.
        BuildPipeline.BuildPlayer(levels, 
            BuildPath + $"/{info.AppName}.exe", 
            BuildTarget.StandaloneWindows, 
            info.DevBuild?BuildOptions.Development:BuildOptions.None);
        BuildProgress = 1f / 3;
        FastZip fzip = new FastZip();
        fzip.CreateEmptyDirectories = true;
        fzip.CompressionLevel = Deflater.CompressionLevel.BEST_COMPRESSION;
        fzip.CreateZip($"{BuildPath}.zip",BuildPath,true, "");
        BuildProgress = 2f / 3;
        AppInfoFileIO.WriteAppInfoFile(info, $"{AppUpdatorSettings.Settings.AppBuildPath}/AppInfo.{Constants.APPINFO_FILE_EXT}");
        BuildProgress = 3f / 3;
        BuildInProgress = false;
        
        Directory.Delete(BuildPath, true);
    }
}
