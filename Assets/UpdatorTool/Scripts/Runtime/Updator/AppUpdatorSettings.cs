using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
#endif

namespace UpdatorTool.Scripts.Runtime
{
    [CreateAssetMenu(fileName = "AppUpdatorSettings", menuName = "App Updator/Create Settings Asset")]
    public class AppUpdatorSettings : ScriptableObject
    {
        private const string SCRIPTING_SYMB_RUNINEDTOR = "BYPASS_UPDATE_CHECK_IN_EDITOR";
        
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

        public bool bypassCheckInEditor=true;
        public void GenerateAppInfo()
        {
            AppInfoFileIO.WriteAppInfoFile(CurrentVersion);
        }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            String[] scriptingDefines;
            var target = EditorUserBuildSettings.activeBuildTarget;
            var group = BuildPipeline.GetBuildTargetGroup(target);
            var namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(group);
            PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget, out scriptingDefines);
            if (bypassCheckInEditor)
            {
                if (!scriptingDefines.Contains("BYPASS_UPDATE_CHECK_IN_EDITOR"))
                {
                    List<String> newDefines = new List<string>(scriptingDefines);
                    newDefines.Add(SCRIPTING_SYMB_RUNINEDTOR);
                    PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget, newDefines.ToArray());
                }
            }
            else
            { 
                if (scriptingDefines.Contains("BYPASS_UPDATE_CHECK_IN_EDITOR"))
                {
                    List<String> newDefines = new List<string>(scriptingDefines);
                    newDefines.Remove(SCRIPTING_SYMB_RUNINEDTOR);
                    PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget, newDefines.ToArray());
                }
            }
        }
        #endif
    }
}