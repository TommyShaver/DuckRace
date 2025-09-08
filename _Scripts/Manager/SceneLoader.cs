using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance { get; private set; }

    public enum LoadedScene
    {
        DuckRace,
        LoadingScene,
        OpeningScene,
        UIScene,
        FirstTimeLoad
    };

    public LoadedScene CurrentScene { get; private set; }  

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
    }

    private void Start()
    {
       SceneManager.LoadSceneAsync("OpeningScene", LoadSceneMode.Additive);
    }

    //? Main Logic ................................................................
    public void AdvanceToNextScene(LoadedScene pickscene)
    {
        string sceneToload = pickscene.ToString();
        switch(sceneToload)
        {
            case "DuckRace":
                SceneManager.LoadSceneAsync("UIScene", LoadSceneMode.Additive);
                StartCoroutine(LoadMainScene());
                break;
            case "LoadingScene":
                CheckFirstLoad(); // check if first load is true. If someone was to clear json save file will mess with laoding. 
                break;
            case "OpeningScene":
                SceneManager.LoadSceneAsync("OpeningScene", LoadSceneMode.Additive);
                break;
            case "UIScene":
                SceneManager.LoadSceneAsync("UIScene", LoadSceneMode.Additive);
                break;
        }
    }

    public void UnloadLastScene(LoadedScene unloadscene)
    {
        string sceneToUnload = unloadscene.ToString();
        Scene currentScene = SceneManager.GetSceneByName(sceneToUnload);
        Debug.Log(sceneToUnload);
        if (currentScene.isLoaded)
        {
            Debug.Log("MyGameScene is currently loaded and active.");
            SceneManager.UnloadSceneAsync(sceneToUnload);
        }
    }
    
    private void CheckFirstLoad()
    {
        if (SaveDataManager.instance.firstLoad)
        {
            SceneManager.LoadSceneAsync("FirstTimeLoad", LoadSceneMode.Additive);
            return;
        }
        SceneManager.LoadSceneAsync("LoadingScreen", LoadSceneMode.Additive);
    }

    private IEnumerator LoadMainScene()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadSceneAsync("DuckRace", LoadSceneMode.Additive);
    }
}
