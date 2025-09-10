using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public bool canGo;
    

    [SerializeField] private Dictionary<string, float> duckLocation = new Dictionary<string, float>();
    [SerializeField] private Dictionary<string, float> duckSpeed = new Dictionary<string, float>();

    //private List<string> nameStored = new List<string>();
    private bool winner;

    private bool stopTracking;
    private bool gameAutomatic;
    private float timer = .5f;
    private Coroutine autoStartCoroutine;

    private string[] helperTips = {"You can join the game by typing in Twitch chat !join.",
                                   "Check out more menu items by pressing the left control.",
                                   "Feeling spicy? !taunt during your next game.",
                                   "There is a 5% chance of a holo duck on spawn.",
                                   "All commands are listed on the itch.io page.",
                                   "Don't want the auto-play? !stop will stop that.",
                                   "Found a bug? Please let me know in the reviews."};

    public static event Action OnClearPlayers;
    public static event Action OnResetPlayers;
    public static event Action OnDucksGo;
    public static event Action OnStopDucks;
    public static event Action OnGrabLocation;
    public static event Action OnWinner;
    public static event Action OnWinAnimationCleanup;
    public static event Action OnShowScoreBorad;
    public static event Action<string, Color, bool, bool> OnWinAnimation;
    public static event Action<string> OnGetWinnerName;
    public static event Action OnStopAutoPlay;
    public static event Action OnRockPlacementUpdate;
 

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
        CameraShotsScript.instance.ChangeCamera();
        PlayerWarningTextScript.instance.CleanUpWarningMessage();
        gameAutomatic = SaveDataManager.instance.autoPlayGo;
    }


    private void Update()
    {

        timer -= Time.deltaTime;
        if (timer <= 0.0f && !stopTracking)
        {
            timer = 0.5f;
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
    public void DucksCrossedFinishLine(string names, string hat, Color color, bool isRainbow, bool isHolo)
    {
        //nameStored.Add(names);
        if (!winner)
        {
            GameWinner();
            OnWinAnimation?.Invoke(hat, color, isRainbow, isHolo);
            OnGetWinnerName?.Invoke(names);
            winner = true;
            ScoreboradManager.instance.GivePoint(names);
            SoundManager.instance.PlaySFXFromSoundEffect(SoundManager.SFX_Clip.TinklesSound);
            SoundManager.instance.WinnerSoundEffect();
            SoundManager.instance.PlaySFXFromSoundEffect(SoundManager.SFX_Clip.Winner_Clapping);
        }
    }

    private void GameWinner()
    {
        OnWinner?.Invoke();
        OnStopDucks?.Invoke();
        stopTracking = true;
        TwitchEventListner.instance.GameManagerCanRemove(true);
        StartCoroutine(WaitForEnd());
        CameraManager.instance.StopCamera();
        PlayerWarningTextScript.instance.CleanUpWarningMessage();
        SoundManager.instance.StopStartWaterSFX(true);
        SoundManager.instance.PlaySFXFromSoundEffect(SoundManager.SFX_Clip.WINNER_HORN_SFX);
        MusicManager.instance.PlayMusicFromSoundEffect(MusicManager.Music_Clip.WINNER_MUSIC);
        MusicManager.instance.FadoutMainMusic();
        SoundManager.instance.FadeOutCheering();
    }

    //? int Main if you will.-------------------
    public void GameStart()
    {
        bool hasValues = SpawnManager.Instance.usernameLog.Any(name => !string.IsNullOrEmpty(name));
        if (canGo && hasValues)
        {
            StartCoroutine(CountDownToGo());
            canGo = false;
            CameraShotsScript.instance.TurnOffCamera();
            TwitchEventListner.instance.GameManagerCanRemove(false);
            MusicManager.instance.FadeOutLoadingMusic();
        }
    }
    public void RemovePlayer(string nameOfDuck)
    {
        duckLocation.Remove(nameOfDuck);
        duckSpeed.Remove(nameOfDuck);
    }

    public void GameReset()
    {
        StopAllCoroutines();
        canGo = true;
        winner = false;
        stopTracking = true;
        OnResetPlayers?.Invoke(); //Duck Manager
        autoStartCoroutine = StartCoroutine(RedoGame());
        CameraManager.instance.GameReset();
        SpawnManager.Instance.CanSpawn(true);
        WaterTrapSpawner.Instance.WaterTrapsDespawn();
        WaterTrapSpawner.Instance.ClearPos();
        SpawnManager.Instance.TryToSpawnAgain();
        CameraShotsScript.instance.ChangeCamera();
        PlayerWarningTextScript.instance.CleanUpWarningMessage();
        SoundManager.instance.FilterOutAudio(false);
        SoundManager.instance.FadeOutAudio(false);
        SoundManager.instance.StopStartWaterSFX(false);
        MusicManager.instance.StopAllPlayers();
        MusicManager.instance.PlayMusicFromSoundEffect(MusicManager.Music_Clip.Loading_Music);
        TwitchEventListner.instance.GameManagerCanRemove(true);
        if (SaveDataManager.instance.autoPlayGo)
        {
            PlayerWarningTextScript.instance.SwitchPlayerWarningText("Game starting soon!");
        }
    }

    public void GameClear()
    {
        StopAllCoroutines();
        canGo = true;
        winner = false;
        stopTracking = true;
        OnClearPlayers?.Invoke(); //Duck Manager
        //nameStored.Clear();
        duckLocation.Clear();
        duckSpeed.Clear();
        CameraManager.instance.GameReset();
        SpawnManager.Instance.ResetSpawnCount();
        WaterTrapSpawner.Instance.WaterTrapsDespawn();
        WaterTrapSpawner.Instance.ClearPos();
        SpawnManager.Instance.CanSpawn(true);
        CameraShotsScript.instance.ChangeCamera();
        PlayerWarningTextScript.instance.CleanUpWarningMessage();
        ScoreboradManager.instance.ClearName();
        SoundManager.instance.PlaySFXFromSoundEffect(SoundManager.SFX_Clip.Clear_Duck_SFX);
        SoundManager.instance.FilterOutAudio(false);
        SoundManager.instance.FadeOutAudio(false);
        SoundManager.instance.StopStartWaterSFX(false);
        MusicManager.instance.StopAllPlayers();
        TwitchEventListner.instance.GameManagerCanRemove(true);
        MusicManager.instance.PlayMusicFromSoundEffect(MusicManager.Music_Clip.Loading_Music);
    }

    public void StopAutoPlay()
    {
        gameAutomatic = false;
        OnStopAutoPlay?.Invoke();
        if (autoStartCoroutine != null)
        {
            StopCoroutine(autoStartCoroutine);
            autoStartCoroutine = null;
        }
    }


    private IEnumerator WaitForEnd()
    {
        yield return new WaitForSeconds(7);
        OnShowScoreBorad?.Invoke();
        MusicManager.instance.PlayMusicFromSoundEffect(MusicManager.Music_Clip.Scoreborad_Music);
        yield return new WaitForSeconds(15);
        GameReset();
        OnWinAnimationCleanup?.Invoke();
    }

    private IEnumerator RedoGame()
    {
        yield return new WaitForSeconds(5);
        int randomString = UnityEngine.Random.Range(0, helperTips.Length);
        PlayerWarningTextScript.instance.SwitchPlayerWarningText(helperTips[randomString]);
        yield return new WaitForSeconds(5);
        if (gameAutomatic)
        {
            GameStart();
        }
    }


    //Count down for game start.
    private IEnumerator CountDownToGo()
    {
        SoundManager.instance.PlaySFXFromSoundEffect(SoundManager.SFX_Clip.START_GAME_SFX);
        yield return new WaitForSeconds(1f); //clear UI
        PlayerWarningTextScript.instance.CleanUpWarningMessage();
        yield return new WaitForSeconds(1f);
        int count;
        for (count = 0; count <= 3; count++) //Start funciton each call happens 1.2 seconds
        {
            Debug.Log("Did We get stuck here game manager? Before switch");
            switch (count)
            {
                case 0:
                    Debug.Log("Did We get stuck here game manager? Before during case 0 start");
                    WaterTrapSpawner.Instance.WaterTrapsTransformUpdate();
                    CameraManager.instance.SetCameraForRace(); //This will update the spawn postion of water speed traps.
                    CountDownScirpt.instance.ChangeCountDownText("3");
                    MusicManager.instance.PlayMusicFromSoundEffect(MusicManager.Music_Clip.Count_Down_SFX);
                    Debug.Log("Did We get stuck here game manager? Before during case 0 end");
                    break;
                case 1:
                    SpawnManager.Instance.CanSpawn(false);
                    CountDownScirpt.instance.ChangeCountDownText("2");
                    MusicManager.instance.PlayMusicFromSoundEffect(MusicManager.Music_Clip.Count_Down_SFX);
                    OnRockPlacementUpdate?.Invoke();
                    break;
                case 2:
                    WaterTrapSpawner.Instance.WaterTrapsAnimationUp();
                    SoundManager.instance.PlaySFXFromSoundEffect(SoundManager.SFX_Clip.Waves_Spawn);
                    MusicManager.instance.PlayMusicFromSoundEffect(MusicManager.Music_Clip.Count_Down_SFX);
                    CountDownScirpt.instance.ChangeCountDownText("1");
                    break;
                case 3:
                    OnDucksGo?.Invoke(); //Duck Manager
                    CountDownScirpt.instance.ChangeCountDownText("GO!");
                    MusicManager.instance.PlayMusicFromSoundEffect(MusicManager.Music_Clip.GO_SFX);
                    stopTracking = false;
                    CameraShotsScript.instance.DoubleCheck();
                    CameraManager.instance.WaitAndGo();
                    SoundManager.instance.PlaySFXFromSoundEffect(SoundManager.SFX_Clip.WINNER_HORN_2_SFX);
                    MusicManager.instance.PlayMusicFromSoundEffect(MusicManager.Music_Clip.Duck_Main_Theme);
                    break;
            }
            yield return new WaitForSeconds(1.5f);
            int activeTweens = DOTween.TotalActiveTweens();
            Debug.Log($"Active Tween: {activeTweens}");
        }
    }
}
