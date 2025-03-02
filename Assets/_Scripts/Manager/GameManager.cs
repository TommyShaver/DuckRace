using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public ParticleSystem confetti;

    private Dictionary<string, float> duckLocation = new Dictionary<string, float>();
    private Dictionary<string, float> duckSpeed = new Dictionary<string, float>();

    private List<string> nameStored = new List<string>();
    private bool winner;

    private bool resetCalled;
    private bool stopTracking;
    private string hotStreakName;
    private float timer = .5f;

    public static event Action OnClearPlayers;
    public static event Action OnResetPlayers;
    public static event Action OnDucksGo;
    public static event Action OnStopDucks;
    public static event Action OnGrabLocation;

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
        confetti.Stop();
        stopTracking = true;
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
            if (hotStreakName == names)
            {
                //check to see if its the same person.
                UIManager.Instance.hotStreakText.text = "Cheating: " + hotStreakName;
            }
            else
            {
                hotStreakName = names;
                UIManager.Instance.hotStreakText.text = string.Empty;
            }
        }
        UIManager.Instance.NamePlateLogic(names);
    }

    private void GameWinner()
    {
        stopTracking = true;
        confetti.Play();
        resetCalled = false;
        StartCoroutine(WaitForEnd());
        UIManager.Instance.joinText.text = "WINNER!";
        UIManager.Instance.winnerNameText.text = nameStored[0] + "!";
        UIManager.Instance.GameOverButtons(true);
        UIManager.Instance.EndOfGameBorder();
        //Play sound effect
        //wait a minute easter egg
    }

    //Public UI Methods (This is called via twitch commadns or code.) ---------------------
    public void GameStart()
    {
        OnDucksGo?.Invoke(); //Duck Manager
        stopTracking = false;

        CameraManager.instance.WaitAndGo();
        SpawnManager.Instance.CanSpawn(false);
        TwitchManager.instance.gameStarted = true;
        UIManager.Instance.ShowHideStartButton(false);
    }

    public void GameReset()
    {
        OnResetPlayers?.Invoke(); //Duck Manager

        nameStored.Clear();
        confetti.Stop();
        winner = false;
        resetCalled = true;
        stopTracking = true;

        CameraManager.instance.GameReset();
        PlayField.instance.DespwanTrapsObject();
        SpawnManager.Instance.CanSpawn(true);
        TwitchManager.instance.gameStarted = false;
        UIManager.Instance.joinText.text = string.Empty;
        UIManager.Instance.winnerNameText.text = string.Empty;
        UIManager.Instance.GameOverButtons(false);
        UIManager.Instance.ShowHideStartButton(true);
        UIManager.Instance.ResetEndOfGameUI();
    }

    public void GameClear()
    {
        OnClearPlayers?.Invoke(); //Duck Manager
        

        confetti.Stop();
        nameStored.Clear();
        duckLocation.Clear();
        duckSpeed.Clear();
        winner = false;
        resetCalled = true;
        stopTracking = true;

        CameraManager.instance.GameReset();
        PlayField.instance.DespwanTrapsObject();
        SpawnManager.Instance.ResetSpawnCount();
        SpawnManager.Instance.CanSpawn(true);
        TwitchManager.instance.gameStarted = false;
        UIManager.Instance.ShowHideStartButton(false);
        UIManager.Instance.GameOverButtons(false);
        UIManager.Instance.joinText.text = "!Join to join the game";
        UIManager.Instance.winnerNameText.text = string.Empty;
        UIManager.Instance.ResetEndOfGameUI();
    }

    private IEnumerator WaitForEnd()
    {
        yield return new WaitForSeconds(15);
        if (!resetCalled)
        {
            OnStopDucks?.Invoke();
            CameraManager.instance.StopCamera();
        }
    }
    //Debug menu -------------------------------------------
}
