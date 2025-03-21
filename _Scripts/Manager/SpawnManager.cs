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
    public void IncomingData(string name) //From Twitch Manager
    {
        if(!canSpawn)
        {
            return;
        }
        if (usernameLog.All(item => !string.IsNullOrEmpty(item)))
        {
            Debug.Log("This game is full");
            return;
        }
        for (int i = 0; i < usernameLog.Length; i++)
        {
            if (usernameLog[i] == name)
            {
                Debug.Log("Name Already in list.");
                Debug.Log(usernameLog[i]);
                return;
            }
            else
            {
                usernameLog[i] = name;
                SpawnPrefab();
                OnSpawn?.Invoke(name, spawnCount); //To Duck Manager
                GameManager.instance.DucksDictinoary(name, 0);
                return;
            }
        }
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
