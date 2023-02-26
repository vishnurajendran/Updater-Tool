using System;
using UnityEngine.Serialization;

namespace UpdatorTool.Scripts.Runtime
{
    [System.Serializable]
    public class AppInfo
    {
        public string AppName = "App 1";
        public string AppVersion = "0.0.1";
        public string BuildVersion = "00001";
        public string Author = "Default Company";
        [FormerlySerializedAs("BuildDate")] public string BuildTime = DateTime.UtcNow.ToString();
        public bool MandatoryUpdate = false;
        public bool DevBuild = false;

        public string FileName =>
            $"{AppVersion}_{BuildVersion}{(DevBuild ? "_dev" : string.Empty)}";
    }
}