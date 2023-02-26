using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UpdatorTool.Scripts.Runtime;

public static class LocalisationManager
{
    private static Dictionary<string, string> localisationHash;
    
    private static void Initialise()
    {
        if (localisationHash != null)
            return;
        
        var txt = Resources.Load<TextAsset>(Constants.LOCALISATION_RESSFILE_PATH).text;
        localisationHash = JsonConvert.DeserializeObject<Dictionary<string,string>>(txt);
        Debug.Log("Localisation Manager Ready");
    }

    public static string GetLocallised(string key, string fallback)
    {
        Initialise();
        
        if (localisationHash == null)
            return fallback;

        if (!localisationHash.ContainsKey(key))
            return fallback;

        return localisationHash[key];
    }
}
