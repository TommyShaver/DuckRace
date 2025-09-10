using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }
    public List<string> storeNames = new List<string>();
    public GameObject duckPrefab;
    [SerializeField] private Vector3[] spawnPoints;

    [SerializeField] private int spawnCount;
    public string[] usernameLog = new string[21];

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
        if (SaveDataManager.instance.CheckBannedPlayers(ducksName))
            return;

        if (usernameLog.All(item => !string.IsNullOrEmpty(item)))
            return; // Check to make sure thre is a spot left

        for (int i = 0; i < usernameLog.Length; i++) //Check to see if the player is already there 
        {
            if (usernameLog[i] == ducksName)
            {
                Debug.Log("Name Already in list." + i);
                return;
            }
        }

        if (!canSpawn) //Check to make sure you can spawn
        {
            storeNames.Add(ducksName);
            return;
        }

        for (int i = 0; i < usernameLog.Length; i++)
        {
            if (usernameLog[i] == string.Empty)
            {
                usernameLog[i] = ducksName;
                spawnCount = i;
                SpawnPrefab();
                OnSpawn?.Invoke(ducksName, spawnCount); //To Duck Manager
                GameManager.instance.DucksDictinoary(ducksName, 0);
                ScoreboradManager.instance.GetNameOnSpawn(ducksName);
                break;
            }
        }
    }


    private void SpawnPrefab()
    {
        Instantiate(duckPrefab, spawnPoints[spawnCount], Quaternion.identity);
        GameManager.instance.canGo = true;
    }

    public void CanSpawn(bool isTrue)
    {
        canSpawn = isTrue;
    }

    public void TryToSpawnAgain()
    {
        if (storeNames == null)
            return;

        StartCoroutine(SlowlySpawn());
    }

    //UI buttons --------------------------------------------------------------------------
    public void ResetSpawnCount()
    {
        spawnCount = 0;
        for (int i = 0; i < usernameLog.Length; i++)
        {
            usernameLog[i] = string.Empty;
        }
    }

    public void ClearPlayerFromSpot(string name)
    {
        for (int i = 0; i < usernameLog.Length; i++)
        {
            if (usernameLog[i] == name)
            {
                usernameLog[i] = string.Empty;
                spawnCount = i;
            }
        }
    }

    private IEnumerator SlowlySpawn()
    {
        foreach (string names in storeNames)
        {
            IncomingData(names);
            yield return new WaitForSeconds(.5f);
        }
        storeNames.Clear();
    }
}
