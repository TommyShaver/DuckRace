using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {  get; private set; }
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

    private void Update()
    {

        //Debug Commands
        TimeMod();
        ResetGame();
    }

    //Debug menu -------------------------------------------
    private void ResetGame()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadScene("MainGameScene");
        }
    }

    private void TimeMod()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Time.timeScale = .5f;
        }
    }
}
