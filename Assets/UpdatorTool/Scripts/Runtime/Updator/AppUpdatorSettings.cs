using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Windows;

namespace UpdatorTool.Scripts.Runtime
{
    [CreateAssetMenu(fileName = "AppUpdatorSettings", menuName = "App Updator/Create Settings Asset")]
    public class AppUpdatorSettings : ScriptableObject
    {
        public static AppUpdatorSettings Settings => Resources.Load<AppUpdatorSettings>("AppUpdatorSettings");

        [System.Serializable]
        public class AppUpdateInfo
        {
            public string UpdateMetaFileUrl;
        }
        
        [FormerlySerializedAs("updateInfo")] [Header("App Update Info")] 
        public AppUpdateInfo UpdateInfo;
        
        public string AppBuildPath;

        [Header("App Current Info")] 
        public AppInfo CurrentVersion;
        
        public void GenerateAppInfo()
        {
            AppInfoFileIO.WriteAppInfoFile(CurrentVersion);
        }
    }
}