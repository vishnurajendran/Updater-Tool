using System.IO;
using Newtonsoft.Json;

namespace UpdatorTool.Scripts.Runtime
{
    public static class AppInfoFileIO
    {
        public static bool HasAppInfoFile()
        {
            return File.Exists(Constants.APPINFO_PATH);
        }
        
        public static AppInfo ReadAppInfoFile()
        {
            if (!HasAppInfoFile())
                return null;

            return Encoder.Decode<AppInfo>(File.ReadAllBytes(Constants.APPINFO_PATH));
        }

        public static void WriteAppInfoFile(AppInfo info)
        {
            WriteAppInfoFile(info, Constants.APPINFO_PATH);
        }
        
        public static void WriteAppInfoFile(AppInfo info, string dest)
        {
            File.WriteAllBytes(dest,Encoder.Encode<AppInfo>(info));
        }
    }
}