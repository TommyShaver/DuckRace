using UnityEngine;
using DG.Tweening;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance { get; private set; }
    public AudioClip[] musicTrack;
    public AudioSource[] musicPlayer;

    private Tween audioFade;
    private Coroutine mainLoop;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        musicPlayer[0].volume = 0;
        IntroLoadingMusicStart();
    }

    public void IntroLoadingMusicStart()
    {
        musicPlayer[0].clip = musicTrack[0];
        musicPlayer[0].Play();
        musicPlayer[0].DOFade(.6f, 3f);
    }

    public void IntroLoadingMusicEnd()
    {
        musicPlayer[0].DOFade(0, 2f);
    }
    public void EndOfGameMusicStart()
    {
        StartCoroutine(WaitTimer());
    }

    public void EndOfGameMusicEnd()
    {
        
        StopCoroutine(WaitTimer());
        musicPlayer[1].DOFade(0, 2f).OnComplete(() =>
            {
                audioFade.Kill();
                musicPlayer[1].Stop();
            });
    }

    // Main music -------------------------------------------------------------------------
    public void MainLoopIntro()
    {
        musicPlayer[0].Stop();
        musicPlayer[1].Stop();
        musicPlayer[2].clip = musicTrack[2];
        musicPlayer[2].volume = .45f;
        musicPlayer[2].PlayDelayed(1);
        mainLoop = StartCoroutine(StartMainLoop());
    }

    private void MainLoopSong()
    {
        musicPlayer[3].clip = musicTrack[3];
        musicPlayer[3].volume = .45f;
        musicPlayer[3].Play();
    }

    public void CleanUpMainLoop()
    {
        musicPlayer[2].DOFade(0, .2f).OnComplete(() =>
        {
            musicPlayer[2].Stop();
        });

        musicPlayer[3].DOFade(0, .2f).OnComplete(() =>
        {
            musicPlayer[3].Stop();
        });
        StopCoroutine(mainLoop);
    }

    private IEnumerator WaitTimer()
    {
        musicPlayer[1].volume = 0;
        yield return new WaitForSeconds(3);
        musicPlayer[1].clip = musicTrack[1];
        musicPlayer[1].Play();
        audioFade = musicPlayer[1].DOFade(.6f, 7);
        Debug.Log("Bottom of Wait Timer");

    }
    private IEnumerator StartMainLoop()
    {
        yield return new WaitForSeconds(2.7f);
        MainLoopSong();
        Debug.Log("Music Manager - StartMainLoop");
    }
}
