using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public ParticleSystem confetti;
    public bool canGo;
    

    private Dictionary<string, float> duckLocation = new Dictionary<string, float>();
    private Dictionary<string, float> duckSpeed = new Dictionary<string, float>();
    private Char[] serectSong;

    private List<string> nameStored = new List<string>();
    private bool winner;
    private bool canSelect;

    private bool resetCalled;
    private bool stopTracking;
    private string hotStreakName;
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
        confetti.Stop();
        stopTracking = true;
        canSelect = true;
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
        OnWinner?.Invoke();

        stopTracking = true;
        confetti.Play();
        resetCalled = false;
        StartCoroutine(WaitForEnd());
        UIManager.Instance.joinText.fontSize = 75;
        UIManager.Instance.joinText.DOFade(1, 0);
        UIManager.Instance.joinText.text = "WINNER!";
        UIManager.Instance.winnerNameText.text = nameStored[0] + "!";
        UIManager.Instance.GameOverButtons(true);
        UIManager.Instance.EndOfGameBorder();
        PlayField.instance.DespwanTrapsObject(false);
        SoundManager.instance.Horn_SFX(11);
        MusicManager.instance.EndOfGameMusicStart();
        MusicManager.instance.CleanUpMainLoop();
        SoundManager.instance.EndGameCheeringPlayer();
        ScoreBoradManager.instance.ScoreUpdate(nameStored[0]);
        ScoreBoradManager.instance.MoveOnScreen();
        
    }

    //Public UI Methods (This is called via twitch commadns or code.) ---------------------
    public void GameStart()
    {
        if(canGo)
        {
            StartCoroutine(CountDownToGo());
            UIManager.Instance.ShowHideStartButton(false);
            canGo = false;
            SoundManager.instance.StartGame_SFX();
            MusicManager.instance.IntroLoadingMusicEnd();
        }
    }

    public void GameReset()
    {
        if(canSelect)
        {
            OnResetPlayers?.Invoke(); //Duck Manager
            OnWinner?.Invoke(); // Reset Remote Sound Players 

            nameStored.Clear();
            confetti.Stop();
            winner = false;
            resetCalled = true;
            stopTracking = true;

            CameraManager.instance.GameReset();
            PlayField.instance.DespwanTrapsObject(true);
            SpawnManager.Instance.CanSpawn(true);
            TwitchManager.instance.gameStarted = false;
            UIManager.Instance.joinText.text = string.Empty;
            UIManager.Instance.winnerNameText.text = string.Empty;
            UIManager.Instance.GameOverButtons(false);
            UIManager.Instance.ShowHideStartButton(true);
            UIManager.Instance.ResetEndOfGameUI();
            StopCoroutine(CountDownToGo());
            canGo = true;
            canSelect = false;
            StartCoroutine(ButtonSelected());
            MusicManager.instance.EndOfGameMusicEnd();
            MusicManager.instance.IntroLoadingMusicStart();
            MusicManager.instance.CleanUpMainLoop();
            SoundManager.instance.playedOnce = false;
            SoundManager.instance.EndGameCheeringPlayer();
            ScoreBoradManager.instance.MoveOffScreen();
        }
    }

    public void GameClear()
    {
        if(canSelect)
        {
            OnClearPlayers?.Invoke(); //Duck Manager
            OnWinner?.Invoke(); // Reset Remote Sound Players

            confetti.Stop();
            nameStored.Clear();
            duckLocation.Clear();
            duckSpeed.Clear();
            winner = false;
            resetCalled = true;
            stopTracking = true;

            CameraManager.instance.GameReset();
            PlayField.instance.DespwanTrapsObject(true);
            SpawnManager.Instance.ResetSpawnCount();
            SpawnManager.Instance.CanSpawn(true);
            TwitchManager.instance.gameStarted = false;
            UIManager.Instance.ShowHideStartButton(false);
            UIManager.Instance.GameOverButtons(false);
            UIManager.Instance.joinText.fontSize = 75;
            UIManager.Instance.joinText.text = "!Join to join the game";
            UIManager.Instance.joinText.DOFade(1, 0);
            UIManager.Instance.winnerNameText.text = string.Empty;
            UIManager.Instance.ResetEndOfGameUI();
            UIManager.Instance.resetButtonMenu.SetActive(false);
            StopCoroutine(CountDownToGo());
            canGo = true;
            canSelect = false;
            StartCoroutine(ButtonSelected());
            MusicManager.instance.EndOfGameMusicEnd();
            MusicManager.instance.IntroLoadingMusicStart();
            MusicManager.instance.CleanUpMainLoop();
            SoundManager.instance.playedOnce = false;
            SoundManager.instance.EndGameCheeringPlayer();
            ScoreBoradManager.instance.MoveOffScreen();
            ScoreBoradManager.instance.CleanUp();
        }
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

    private IEnumerator CountDownToGo()
    {
        yield return new WaitForSeconds(.5f);
        int count;
        for(count = 0; count <= 3; count++)
        {
           switch(count)
            {
                case 0:
                    UIManager.Instance.AnimateCountDownText("3", .5f);
                    SoundManager.instance.Horn_SFX(9);
                    //Play Sound
                    break;
                case 1:
                    UIManager.Instance.AnimateCountDownText("2", .5f);
                    PlayField.instance.StartSpawn();
                    SoundManager.instance.Horn_SFX(9);
                    //Play Sound
                    break;
                case 2:
                    UIManager.Instance.AnimateCountDownText("1", .5f);
                    SoundManager.instance.Horn_SFX(9);
                    //Play Sound
                    break;
                case 3:
                    UIManager.Instance.AnimateCountDownText("GO!", 1.2f);
                    SoundManager.instance.Horn_SFX(10);
                    //Play Go Sound
                    OnDucksGo?.Invoke(); //Duck Manager
                    stopTracking = false;
                    CameraManager.instance.WaitAndGo();
                    SpawnManager.Instance.CanSpawn(false);
                    TwitchManager.instance.gameStarted = true;
                    MusicManager.instance.MainLoopIntro();
                    break;
            }    
            yield return new WaitForSeconds(1.2f);
        }
    }
    private IEnumerator ButtonSelected() 
    {
        yield return new WaitForSeconds(.5f);
        canSelect = true;
    }

    //Debug menu -------------------------------------------
}
