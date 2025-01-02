using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }
    public GameObject duckPrefab;
    public Vector3[] spawnPoints;

    [SerializeField] private int spawnCount;

    [SerializeField] private string[] usernameLog = new string[8];

    public static event Action<string> OnSpawn;

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
        foreach (string s in usernameLog)
        {
            if (!string.IsNullOrEmpty(s))
            {
                Debug.Log("This game is full");
                return;
            }
        }
        for (int i = 0; i < usernameLog.Length; i++)
        {
            if (usernameLog[i] == name)
            {
                Debug.Log("Name Already in list.");
                return;
            }
            else
            {
                usernameLog[i] = name;
                SpawnPrefab();
                OnSpawn?.Invoke(name); //To Duck Manager
                Debug.Log(name + " <- user name Spawn Manager");
                return;
            }
        }
    }
    private void SpawnPrefab()
    {
        Instantiate(duckPrefab, spawnPoints[spawnCount], Quaternion.identity);
        spawnCount++;
    }


    //UI buttons --------------------------------------------------------------------------
    public void ResetSpawnCount()
    {
        spawnCount = 0;
        for (int i = 0; i < usernameLog.Length; i++)
        {
            usernameLog[i] = null;
        }
    }


}
