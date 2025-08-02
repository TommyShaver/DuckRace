using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public bool canGo;
    

    private Dictionary<string, float> duckLocation = new Dictionary<string, float>();
    private Dictionary<string, float> duckSpeed = new Dictionary<string, float>();

    private List<string> nameStored = new List<string>();
    private bool winner;

    private bool stopTracking;
    private bool gameAutomatic;
    private float timer = .5f;

    public static event Action OnClearPlayers;
    public static event Action OnResetPlayers;
    public static event Action OnDucksGo;
    public static event Action OnStopDucks;
    public static event Action OnGrabLocation;
    public static event Action OnWinner;
 

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


        canGo = false;
        stopTracking = true;
    }

    private void Start()
    {
        CameraShotsScript.instance.ChangeCamera();       
    }


    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0.0f && !stopTracking)
        {
            timer = .5f;
            OnGrabLocation?.Invoke();
        }
    }

    //Get location of ducks ---------------------------------------------------------------
    public void DucksDictinoary(string userName, float speed)
    {
        duckLocation.Add(userName, 0);
        duckSpeed.Add(userName, 0);
    }

    public void DucksSpeed(string userName, float speed)
    {
        duckSpeed[userName] = speed;
    }

    public void WhoIsFisrt(string userName, float distance)
    {
        duckLocation[userName] = distance;
        float minValue = duckLocation.Values.Min();
        var minEntry = duckLocation.FirstOrDefault(x => x.Value == minValue);
        CameraManager.instance.GrabCurrentSpeed(duckSpeed[minEntry.Key]);
    }

    //Grab winner of game -----------------------------------------------------------------
    public void DucksCrossedFinishLine(string names)
    {
        nameStored.Add(names);
        if (!winner)
        {
            GameWinner();
            winner = true;
        }
    }

    private void GameWinner()
    {
        OnWinner?.Invoke();
        OnStopDucks?.Invoke();
        stopTracking = true;
        StartCoroutine(WaitForEnd());
        CameraManager.instance.StopCamera();
    }

    //? int Main if you will.-------------------
    public void GameStart()
    {
        if (canGo)
        {
            StartCoroutine(CountDownToGo());
            canGo = false;
            CameraShotsScript.instance.TurnOffCamera();
        }
    }

    public void GameReset()
    {

        canGo = true;
        winner = false;
        stopTracking = true;
        OnResetPlayers?.Invoke(); //Duck Manager
        CameraManager.instance.GameReset();
        SpawnManager.Instance.CanSpawn(true);
        WaterTrapSpawner.Instance.WaterTrapsDespawn();
        SpawnManager.Instance.TryToSpawnAgain();
        CameraShotsScript.instance.ChangeCamera();   
    }

    public void GameClear()
    {
        canGo = true;
        winner = false;
        stopTracking = true;
        OnClearPlayers?.Invoke(); //Duck Manager
        nameStored.Clear();
        duckLocation.Clear();
        duckSpeed.Clear();
        CameraManager.instance.GameReset();
        SpawnManager.Instance.ResetSpawnCount();
        WaterTrapSpawner.Instance.WaterTrapsDespawn();
        SpawnManager.Instance.CanSpawn(true);
        CameraShotsScript.instance.ChangeCamera();   
    }


    private IEnumerator WaitForEnd()
    {
        yield return new WaitForSeconds(15);
        GameReset();
        if (gameAutomatic)
        {
            GameStart();
        }
    }


    //Count down for game start.
    private IEnumerator CountDownToGo()
    {
        yield return new WaitForSeconds(.5f); //clear UI
        int count;
        for (count = 0; count <= 3; count++) //Start funciton each call happens 1.2 seconds
            {
                switch (count)
                {
                    case 0:
                        WaterTrapSpawner.Instance.WaterTrapsTransformUpdate();
                        CameraManager.instance.SetCameraForRace(); //This will update the spawn postion of water speed traps.
                    break;
                    case 1:
                        SpawnManager.Instance.CanSpawn(false);
                        break;
                    case 2:
                        WaterTrapSpawner.Instance.WaterTrapsAnimationUp();
                    break;
                    case 3:
                        OnDucksGo?.Invoke(); //Duck Manager
                        stopTracking = false;
                        CameraManager.instance.WaitAndGo();
                        break;
                }
                yield return new WaitForSeconds(1.2f);
            }
    }
}
