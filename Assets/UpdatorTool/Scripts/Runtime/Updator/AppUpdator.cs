using System;
using UnityEngine;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;
using UpdatorTool.Scripts.Runtime;

public static class AppUpdator
{
       private static AppUpdatorSettings settings;
       public static Action OnUpdateCheckComplete { get; set; }
       public static AppInfoGUI gui;
       
       private static bool HasAppInfo => AppInfoFileIO.HasAppInfoFile();
       private static AppInfo MyAppInfo => AppInfoFileIO.ReadAppInfoFile();
       
       [RuntimeInitializeOnLoadMethod]
       private static async void CheckForUpdates()
       {
              Debug.Log("App Updater Initialised");
              if (settings == null)
                     settings = AppUpdatorSettings.Settings;
              
              Debug.Assert(settings != null,"Settings for App Updator is not available.");
              if (!HasAppInfo)
                     settings.GenerateAppInfo();

              AppInfoGUI.Instance.AppVersion = MyAppInfo.AppVersion + $"b {MyAppInfo.BuildVersion}" +
                                               $"-{MyAppInfo.BuildTime}{(MyAppInfo.DevBuild?" (Dev)":string.Empty)}";
              
              var currAppInfo = MyAppInfo;
              
#if UNITY_EDITOR && BYPASS_UPDATE_CHECK_IN_EDITOR
              OnUpdateCheckComplete?.Invoke();
#else
              Debug.Log("Checking for Update");
              AppInfoGUI.Instance.UpdateCheckGUIVisible = true;
              var newAppInfo = await GetRemoteAppInfo();
              AppInfoGUI.Instance.UpdateCheckGUIVisible = false;
              if (newAppInfo.AppVersion != currAppInfo.AppVersion)
              {
                     TryDoUpdate(newAppInfo);
              }
              else
              {
                     if(newAppInfo.BuildVersion != currAppInfo.BuildVersion)
                            TryDoUpdate(newAppInfo);
                     else
                          OnUpdateCheckComplete?.Invoke();
              }
#endif
       }

       private static void TryDoUpdate(AppInfo appInfo)
       {
              if (appInfo.MandatoryUpdate)
              {
                     Debug.Log("Mandatory Update detected!!");
                     DoUpdate(appInfo);
              }
              else
                     ShowUpdatePrompt(appInfo);
       }

       private static void ShowUpdatePrompt(AppInfo appInfo)
       {
              AppInfoGUI.Instance.ShowUpdatePrompt(()=>DoUpdate(appInfo), null);
       }
       
       private static async void DoUpdate(AppInfo appInfo)
       {
              var basePath = $"{Path.GetDirectoryName(AppUpdatorSettings.Settings.UpdateInfo.UpdateMetaFileUrl)}";
              var path = $"{basePath.Replace("\\","//")}/{appInfo.FileName}.zip";
              Debug.Log($"App Update Started {path}");
              using HttpClientDownloadWithProgress client = new HttpClientDownloadWithProgress(path, $"{appInfo.FileName}.zip");
              client.ProgressChanged += (totalSize, downloaded, progress) =>
              {
                     AppInfoGUI.Instance.UpdateDownloadProgress = (float)progress;
                     if (downloaded >= totalSize)
                     {
                            AppInfoGUI.Instance.UpdateDownloadProgressVisible = false;
                     }
              };
              AppInfoGUI.Instance.UpdateDownloadProgressVisible = true;
              await client.StartDownload();
              UnpackUpdate(appInfo);
              Debug.Log("Updating local metadata");
              AppInfoFileIO.WriteAppInfoFile(appInfo);
              Debug.Log("Restarting App");
              System.Diagnostics.Process.Start($"{Application.productName}.exe"); 
              Application.Quit();
       }

       private static void UnpackUpdate(AppInfo appInfo)
       {
              var updateFileName = $"{appInfo.FileName}.zip";
              FastZip fzip = new FastZip();
              fzip.ExtractZip(updateFileName,Application.dataPath,"");
              File.Delete(updateFileName);
       }

       private static async Task<AppInfo> GetRemoteAppInfo()
       {
              await Task.Delay(1500);
              using HttpClient client = new HttpClient();
              var resp = await client.GetAsync(settings.UpdateInfo.UpdateMetaFileUrl);
              resp.EnsureSuccessStatusCode();
              Debug.Log("Update Meta Retrieved");
              var bytes = await resp.Content.ReadAsByteArrayAsync();
              return Encoder.Decode<AppInfo>(bytes);
       }
}
