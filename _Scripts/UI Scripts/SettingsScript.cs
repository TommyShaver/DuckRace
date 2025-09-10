using UnityEngine;
using System.Collections;
using DG.Tweening;


public class SettingsScript : MonoBehaviour
{
    public static SettingsScript instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
            Application.targetFrameRate = -1;
           // QualitySettings.vSyncCount = 0;
  
    }
    

    public string SwitchScreenSize()
    {
        string currentFullScreenMode = "";
        if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            currentFullScreenMode = "Windowed";
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            currentFullScreenMode = "Full Screen";
        }

        return currentFullScreenMode;
    }

    public void CloseApp()
    {
        bool hasClicked = false;
        if (hasClicked)
        {
            return;
        }

        hasClicked = true;
        StartCoroutine(CleanUpTmer());
    }

    private IEnumerator CleanUpTmer()
    {
        SaveDataManager.instance.SaveGameData();
        yield return new WaitForSeconds(2);
        Application.Quit();
    }
}
