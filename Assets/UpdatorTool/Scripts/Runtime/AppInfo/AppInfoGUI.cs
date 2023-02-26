using System;
using UnityEngine;
using UnityEngine.UI;

namespace UpdatorTool.Scripts.Runtime
{
    public class AppInfoGUI : MonoBehaviour
    {
        private const string VER_STRING_PREFIX = "app_version_str";
        
        [SerializeField] private TMPro.TMP_Text versionText;

        [SerializeField] private GameObject updatePrompt;
        [SerializeField] private Button yesUpdateBttn;
        [SerializeField] private Button noUpdateBttn;
        
        [SerializeField] private GameObject updateCheckGUI;
        
        [SerializeField] private GameObject updateDownloadGUI;
        [SerializeField] private Image updateProgress;
        [SerializeField] private TMPro.TMP_Text updateProgressText;
        
        private static AppInfoGUI instance;

        public static AppInfoGUI Instance
        {
            get
            {
                if (instance == null)
                    instance = Instantiate(Resources.Load<GameObject>("AppInfoGUI").GetComponent<AppInfoGUI>());

                return instance;
            }
        }

        public string AppVersion
        {
            get => versionText.text.Replace(LocalisationManager.GetLocallised(VER_STRING_PREFIX,"version: "), "");
            set => versionText.text = $"{LocalisationManager.GetLocallised(VER_STRING_PREFIX,"version: ")}{value}";
        }

        public bool UpdateCheckGUIVisible
        {
            get => updateCheckGUI.activeSelf;
            set => updateCheckGUI.SetActive(value);
        }

        public bool UpdateDownloadProgressVisible
        {
            get => updateDownloadGUI.activeSelf;
            set => updateDownloadGUI.SetActive(value);
        }
        
        public float UpdateDownloadProgress
        {
            get => updateProgress.fillAmount * 100f;
            set
            {
                updateProgress.fillAmount = value / 100;
                updateProgressText.text = $"{(int)value}%";
            }
        }
        
        public void ShowUpdatePrompt(Action onYes, Action onNo)
        {
            yesUpdateBttn.onClick.RemoveAllListeners();
            yesUpdateBttn.onClick.AddListener(() =>
            {
                updatePrompt.SetActive(false);
                onYes?.Invoke();
            });
            
            noUpdateBttn.onClick.RemoveAllListeners();
            noUpdateBttn.onClick.AddListener(() =>
            {
                updatePrompt.SetActive(false);
                onNo?.Invoke();
            });
            
            updatePrompt.SetActive(true);
        }
    }
}