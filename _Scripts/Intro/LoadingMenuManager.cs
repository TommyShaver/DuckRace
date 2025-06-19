using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadingMenuManager : MonoBehaviour
{
    public GameObject duckGameObject;
    public GameObject introText;
    public GameObject introStartButton;
    public GameObject introCloseAppButton;

    public IntroDuckAnimation duckAnimation;
    public IntroAudioManager introAudioManager;
    public TextBounce textBounce;

    public static event Action OnButtonClick;

    private bool buttonClickedOnce;
    private AsyncOperation asyncLoadLevel;

    private void Start()
    {
        introText.SetActive(false);
        introStartButton.SetActive(false);
        introCloseAppButton.SetActive(false);
        StartCoroutine(PreLoadScene());
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("OverlayScene", LoadSceneMode.Additive);
    }

    private void OnEnable()
    {
        CutOffMaskAnimation.OnAnimationComplete += SceneUnloaded;
    }
    private void OnDisable()
    {
        CutOffMaskAnimation.OnAnimationComplete -= SceneUnloaded;
    }

    public void ShowLogo()
    {
        introText.SetActive(true);
        duckGameObject.SetActive(true);
        introStartButton.SetActive(true);
        introCloseAppButton.SetActive(true);
        introAudioManager.UI_Loaded();
    }

    //Loading Scene -------------------------
    private void LoadMainGame()
    {
        asyncLoadLevel = SceneManager.LoadSceneAsync("MainGameScene", LoadSceneMode.Additive);
        asyncLoadLevel.allowSceneActivation = false;
    }


    //Unload Scenes ----------------------------
    private void SceneUnloaded()
    {
        duckAnimation.UnloadDuck();
        SceneManager.UnloadSceneAsync("LoadingSene");
        asyncLoadLevel.allowSceneActivation = true;
    }

    //Button Functions -------------------------
    public void PlayGameButton()
    {
        if (buttonClickedOnce)
        {
            return;
        }
        introAudioManager.GameStartSFX();
        //cutOffMaskAnimation.AnimStart();
        buttonClickedOnce = true;
        OnButtonClick?.Invoke();
        textBounce.CleanUpTween();
    }

    public void CloseAppMainMenu()
    {
        Application.Quit();
    }

    private IEnumerator PreLoadScene()
    {
        yield return new WaitForSeconds(4);
        introAudioManager.MainLoopIntro();
        LoadMainGame();
    }
}
