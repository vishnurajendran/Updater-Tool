using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UpdatorTool.Scripts.Runtime;

[CustomEditor(typeof(AppUpdatorSettings))]
public class AppInfoSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Open AppInfo File Location"))
        {
            EditorUtility.RevealInFinder(Constants.APPINFO_PATH);
        }
    }
}

