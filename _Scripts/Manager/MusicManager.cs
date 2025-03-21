using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.Rendering;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance { get; private set; }
    public AudioClip[] musicTrack;
    public AudioSource[] musicPlayer;

    private Tween audioFade;
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
        musicPlayer[0].DOFade(.5f, 3f);
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
        musicPlayer[2].volume = .4f;
        musicPlayer[2].PlayDelayed(1);
        StartCoroutine(StartMainLoop());
    }

    private void MainLoopSong()
    {
        musicPlayer[3].clip = musicTrack[3];
        musicPlayer[3].volume = .4f;
        musicPlayer[3].Play();
    }

    public void CleanUpMainLoop()
    {
        musicPlayer[3].DOFade(0, .2f).OnComplete(() =>
        {
            musicPlayer[3].Stop();
        });
    }

    private IEnumerator WaitTimer()
    {
        musicPlayer[1].volume = 0;
        yield return new WaitForSeconds(3);
        musicPlayer[1].clip = musicTrack[1];
        musicPlayer[1].Play();
        audioFade = musicPlayer[1].DOFade(.5f, 7);

    }
    private IEnumerator StartMainLoop()
    {
        yield return new WaitForSeconds(2.7f);
        MainLoopSong();
        Debug.Log("Music Manager - StartMainLoop");
    }
}
