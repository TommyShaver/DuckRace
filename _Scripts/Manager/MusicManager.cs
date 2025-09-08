using UnityEngine;
using DG.Tweening;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance { get; private set; }
    public AudioClip[] musicTrack;
    public AudioSource[] musicPlayer;

    [SerializeField] private AudioSource loadingPlayerLoop;
    [SerializeField] private AudioSource intoPlayerMainSong;
    [SerializeField] private AudioSource mainGameLoopSong;

    private Tween fadeTween;
    private Tween loadingMusicFadeTween;

    public enum Music_Clip
    {
        Count_Down_SFX,
        GO_SFX,
        Scoreborad_Music,
        WINNER_MUSIC,
        Loading_Music,
        Duck_Main_Theme
    }
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
        PlayMusicFromSoundEffect(Music_Clip.Loading_Music);
        loadingPlayerLoop.clip = musicTrack[6];
        intoPlayerMainSong.clip = musicTrack[4];
        mainGameLoopSong.clip = musicTrack[5];
    }

    public void StopAllPlayers()
    {
        fadeTween.Kill();
        foreach (AudioSource audio in musicPlayer)
        {
            audio.DOFade(0, .3f).OnComplete(() =>
            {
                audio.Stop();
            });
        }
        //I just want this project done. 
        mainGameLoopSong.DOFade(0, .3f).OnComplete(() =>
        {
            mainGameLoopSong.Stop();
        });
        intoPlayerMainSong.DOFade(0, .3f).OnComplete(() =>
        {
            intoPlayerMainSong.Stop();
        });
        loadingMusicFadeTween = loadingPlayerLoop.DOFade(0, .3f).OnComplete(() =>
        {
             PlayMusicFromSoundEffect(Music_Clip.Loading_Music);
        });
    }


    // All the function ------------------------------------------------------------------
    public void FadeOutLoadingMusic()
    {
        loadingMusicFadeTween = loadingPlayerLoop.DOFade(0, .3f).OnComplete(() =>
        {
            loadingPlayerLoop.Stop();
        });
    }
    public void FadoutMainMusic()
    {
        intoPlayerMainSong.DOFade(0, .1f).OnComplete(() =>
        {
            intoPlayerMainSong.Stop();
        });
        mainGameLoopSong.DOFade(0, .1f).OnComplete(() =>
        {
            mainGameLoopSong.Stop();
        });
    }

    public void PlayMusicFromSoundEffect(Music_Clip clip)
    {
        switch (clip)
        {
            case Music_Clip.Count_Down_SFX:
                AudioCallBackMusic2D(musicPlayer[ReturnPlayer()], musicTrack[0], .8f, 0, 0, 1, 0, false, false);
                break;
            case Music_Clip.GO_SFX:
                AudioCallBackMusic2D(musicPlayer[ReturnPlayer()], musicTrack[1], .8f, 0, 0, 1, 0, false, false);
                break;
            case Music_Clip.Scoreborad_Music:
                AudioCallBackMusic2D(musicPlayer[ReturnPlayer()], musicTrack[2], 1, 0, 0, 1, 0, false, false);
                break;
            case Music_Clip.WINNER_MUSIC:
                AudioCallBackMusic2D(musicPlayer[ReturnPlayer()], musicTrack[3], .6f, 0, 0, 1, .2f, false, false);
                break;
            case Music_Clip.Loading_Music:
                loadingMusicFadeTween.Kill();
                AudioCallBackMusic2D(loadingPlayerLoop, musicTrack[6], .6f, 2, 0, 1, .5f, true, true);
                break;
            case Music_Clip.Duck_Main_Theme:
                AudioCallBackMusic2D(intoPlayerMainSong, musicTrack[4], .5f, 0, 0, 1, 0, false, false);
                AudioCallBackMusic2D(mainGameLoopSong, musicTrack[5], .4f, 0, 0, 1, 3.65f, true, false);
                break;

        }
    }


    //Logic for sound manager --------------------------------------------------------
    /// <summary>
    /// Sets up for out bound call from (volume = 0-1, pan -1 to 1, pitch -3 to 3)
    /// </summary>
    /// <param name="player"></param>
    /// <param name="clip"></param>
    /// <param name="volume"></param>
    /// <param name="fadeDuration"></param>
    /// <param name="pan"></param>
    /// <param name="pitch "></param>
    /// <param name="delayAudio"></param>
    /// <param name="loop"></param>
    /// <param name="fade"></param>
    private void AudioCallBackMusic2D(AudioSource player, AudioClip clip, float volume, float fadeDuration, float pan, float pitch, float delayAudio, bool loop, bool fadeIn)
    {
        Debug.Log("This audio function was called: " + clip);
        player.volume = 1;
        player.panStereo = 0;
        player.pitch = 1;
        player.clip = clip;
        if (fadeIn)
        {
            player.volume = 0;
            fadeTween = player.DOFade(volume, fadeDuration);
        }
        else
        {
            player.volume = volume;
        }
        player.panStereo = pan;
        player.pitch = pitch;
        player.loop = loop;
        player.PlayDelayed(delayAudio);
    }

    /// <summary>
    /// Checks avaible audio Source,
    /// </summary>
    /// <returns></returns>
    public int ReturnPlayer()
    {
        int openAudioPlayer = 0;
        for (int i = 0; i < musicPlayer.Length; i++)
        {
            if (!musicPlayer[i].isPlaying)
            {
                openAudioPlayer = i;
                break;
            }
            else
            {
                Debug.LogWarning("No audio sources available");
            }
        }
        Debug.Log(openAudioPlayer + "Opening Audio Function player");
        return openAudioPlayer;
    }
}
