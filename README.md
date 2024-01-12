![](https://github.com/vishnurajendran/Updater-Tool/blob/main/app%20updator%20settings.png)
# Updator-Tool
 a easy to use app updator for Unity games
 
## Features
 - Easy to setup (NO scene setups needed)
 - Customizable GUI
 - Localization support

## How to use
 - Download and import the unitypackage
 - Goto Assets/UpdatorTool/Resources/AppUpdatorSettings, modify `Update Meta File URL` 
   field and point it to where the AppInfo.info can be downloaded from.
 - Update the `App Build Path` to a location where the build needs to be made.
 - Go to App Updator/Create New Update from the menu bar.
 - Set the parameters for your build and click create update file.
 - Upload the AppInfo and the zip file to the same path `Update Meta File URL` from Step 2 is pointing to.

To check if update system is working, you can de-select `Bypass Check In Editor` field 

NOTE 1: the build zip file needs to stay at the same level as the AppInfo.info file, the path is created
From `Update Meta File URL` path to point to the build.

NOTE 2: This tool currently only supports windows.

NOTE 3: This tool does not do a binary diff. its a plain replace of the binaries, Large projects should 
avoid using this tool.
