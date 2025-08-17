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
    public static event Action OnWinAnimationCleanup;
    public static event Action OnShowScoreBorad;
    public static event Action<string, Color, bool> OnWinAnimation;
    public static event Action<string> OnGetWinnerName;
 

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
        gameAutomatic = SaveDataManager.instance.autoPlayGo;
        Debug.Log(gameAutomatic);
        CameraShotsScript.instance.ChangeCamera();
        PlayerWarningTextScript.instance.CleanUpWarningMessage();
        gameAutomatic = SaveDataManager.instance.autoPlayGo;
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

    private void OnEnable()
    {
        UIButtonScript.OnUIButtonSwitch += AutoGamePlay;
    }
    private void OnDisable()
    {
        UIButtonScript.OnUIButtonSwitch -= AutoGamePlay;
    }

    private void AutoGamePlay(bool isAuto, string command)
    {
        if(command == "auto")
            gameAutomatic = isAuto;
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
    public void DucksCrossedFinishLine(string names, string hat, Color color, bool isRainbow)
    {
        nameStored.Add(names);
        if (!winner)
        {
            GameWinner();
            OnWinAnimation?.Invoke(hat, color, isRainbow);
            OnGetWinnerName?.Invoke(names);
            winner = true;
            ScoreboradManager.instance.GivePoint(names);
        }
    }

    private void GameWinner()
    {
        OnWinner?.Invoke();
        OnStopDucks?.Invoke();
        stopTracking = true;
        StartCoroutine(WaitForEnd());
        CameraManager.instance.StopCamera();
        PlayerWarningTextScript.instance.CleanUpWarningMessage();
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
        StopAllCoroutines();
        canGo = true;
        winner = false;
        stopTracking = true;
        OnResetPlayers?.Invoke(); //Duck Manager
        StartCoroutine(RedoGame());
        CameraManager.instance.GameReset();
        SpawnManager.Instance.CanSpawn(true);
        WaterTrapSpawner.Instance.WaterTrapsDespawn();
        SpawnManager.Instance.TryToSpawnAgain();
        CameraShotsScript.instance.ChangeCamera();   
        PlayerWarningTextScript.instance.CleanUpWarningMessage();
    }

    public void GameClear()
    {
        StopAllCoroutines();
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
        PlayerWarningTextScript.instance.CleanUpWarningMessage();
    }


    private IEnumerator WaitForEnd()
    {
        yield return new WaitForSeconds(7);
        OnShowScoreBorad?.Invoke();
        yield return new WaitForSeconds(15);
        GameReset();
        OnWinAnimationCleanup?.Invoke();
    }

    private IEnumerator RedoGame()
    {
        yield return new WaitForSeconds(5);
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
                    CountDownScirpt.instance.ChangeCountDownText("3");
                    break;
                case 1:
                    SpawnManager.Instance.CanSpawn(false);
                    CountDownScirpt.instance.ChangeCountDownText("2");
                    break;
                case 2:
                    WaterTrapSpawner.Instance.WaterTrapsAnimationUp();
                    CountDownScirpt.instance.ChangeCountDownText("1");
                    break;
                case 3:
                    OnDucksGo?.Invoke(); //Duck Manager
                    CountDownScirpt.instance.ChangeCountDownText("GO!");
                    stopTracking = false;
                    CameraShotsScript.instance.DoubleCheck();
                    CameraManager.instance.WaitAndGo();
                    break;
            }
            yield return new WaitForSeconds(2f);
        }
    }
}
