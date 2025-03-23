using System;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }
    public GameObject duckPrefab;
    public Vector3[] spawnPoints;

    [SerializeField] private int spawnCount;
    [SerializeField] private string[] usernameLog = new string[8];

    public static event Action<string, int> OnSpawn;

    private bool canSpawn = true;

    //Setup ------------------------------------------------------------------------------------
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    //Logic ---------------------------------------------------------------------------------
    public void IncomingData(string ducksName) //From Twitch Manager
    {
        bool nameExist = false;
        if(!canSpawn) //Check to make sure you can spawn
        {
            return;
        }
        if (usernameLog.All(item => !string.IsNullOrEmpty(item))) // Check to make sure thre is a spot left
        {
            Debug.Log("This game is full");
            return;
        }
        for (int i = 0; i < usernameLog.Length; i++) //Check to see if the player is already there 
        {
            if (usernameLog[i] == ducksName)
            {
                Debug.Log("Name Already in list." + i);
                nameExist = true;
                break;
            }
        }
        if (!nameExist) // If name doesn't exist find open spot and spawn duck.
        {
            for (int i = 0; i < usernameLog.Length; i++)
            {
                if (usernameLog[i] == string.Empty)
                {
                    usernameLog[i] = ducksName;
                    SpawnPrefab();
                    OnSpawn?.Invoke(ducksName, spawnCount); //To Duck Manager
                    GameManager.instance.DucksDictinoary(ducksName, 0);
                    ScoreBoradManager.instance.GiveName(ducksName);
                    break;
                }
            }
        }
        nameExist = false; // clean up bool
    }
    private void SpawnPrefab()
    {
        Instantiate(duckPrefab, spawnPoints[spawnCount], Quaternion.identity);
        spawnCount++;
        GameManager.instance.canGo = true;
        UIManager.Instance.ShowHideStartButton(true);
        UIManager.Instance.resetButtonMenu.SetActive(true);
    }

    public void CanSpawn(bool isTrue)
    {
        canSpawn = isTrue;
    }


    //UI buttons --------------------------------------------------------------------------
    public void ResetSpawnCount()
    {
        spawnCount = 0;
        Array.Clear(usernameLog, 0, usernameLog.Length);  

    }
}
