using UnityEngine;

public class AutoLocaliser : MonoBehaviour
{
    private void Start()
    {
        var allTMPS = GetComponentsInChildren<TMPro.TMP_Text>(true);
        if (allTMPS == null) return;
        foreach (var tmp in allTMPS)
        {
            tmp.text = LocalisationManager.GetLocallised(tmp.name, tmp.name);
        }
    }
}
