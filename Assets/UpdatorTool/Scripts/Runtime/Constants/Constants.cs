using UnityEngine;

namespace UpdatorTool.Scripts.Runtime
{
    public static class Constants
    {
        public const string APPINFO_FILE_EXT = "info";

        public static readonly string APPINFO_PATH = $"{Application.persistentDataPath}/AppInfo.{Constants.APPINFO_FILE_EXT}";
        public static readonly string LOCALISATION_RESSFILE_PATH = $"LocalisationHash";
    }
}