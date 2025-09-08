using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    public AudioClip[] audio_SFX;
    public AudioSource[] audioPlayer;
    public AudioMixer audioMixer;
    public AudioSource waterAtmoPlayer;
    public AudioSource warpPipeAudioSource;
    public AudioSource endCheeringSoruce;

    public enum SFX_Clip
    {
        Waves_Away,
        Waves_Spawn,
        Clear_Duck_SFX,
        WATER_ATMO_SFX_UPDATE,
        WarpPipe,
        TinklesSound,
        Winner_Clapping,
        START_GAME_SFX,
        WINNER_HORN_2_SFX,
        WINNER_HORN_SFX,
        End_Cheering_SFX

    }

    private Tween fadeTween;

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

    private void Start()
    {
        FilterOutAudio(false);
        PlaySFXFromSoundEffect(SFX_Clip.WATER_ATMO_SFX_UPDATE);
    }

    //Incoming data-------------------------------------------------------------------
    /// <summary>
    /// Stop all players no matter what, with a .3f fade. 
    /// </summary>
    public void StopAllPlayers()
    {
        fadeTween.Kill();
        foreach (AudioSource audio in audioPlayer)
        {
            warpPipeAudioSource.DOFade(0, .5f);
            audio.DOFade(0, .3f).OnComplete(() =>
            {
                audio.Stop();
            });
        }
    }
    public void FadeOutCheering()
    {
        endCheeringSoruce.DOFade(0, .5f);
    }

    public void PlaySFXFromSoundEffect(SFX_Clip clip)
    {
        switch (clip)
        {
            case SFX_Clip.Waves_Spawn:
                AudioCallBack2D(audioPlayer[ReturnPlayer()], audio_SFX[0], 1.5f, 0, 0, 1, 0, false, false);
                break;
            case SFX_Clip.WATER_ATMO_SFX_UPDATE:
                AudioCallBack2D(waterAtmoPlayer, audio_SFX[3], .4f, 2, 0, 1, 0, true, true);
                break;
            case SFX_Clip.Clear_Duck_SFX:
                AudioCallBack2D(audioPlayer[ReturnPlayer()], audio_SFX[2], 1f, 0, 0, 1, 0, false, false);
                break;
            case SFX_Clip.TinklesSound:
                AudioCallBack2D(audioPlayer[ReturnPlayer()], audio_SFX[5], .5f, 0, 0, 1, 0, false, false);
                break;
            case SFX_Clip.Winner_Clapping:
                AudioCallBack2D(audioPlayer[ReturnPlayer()], audio_SFX[6], .5f, 0, 0, 1, 0, false, false);
                break;
            case SFX_Clip.START_GAME_SFX:
                AudioCallBack2D(audioPlayer[ReturnPlayer()], audio_SFX[7], 1f, 0, 0, 1, 0, false, false);
                break;
            case SFX_Clip.WINNER_HORN_SFX:
                AudioCallBack2D(audioPlayer[ReturnPlayer()], audio_SFX[8], 1.5f, 0, 0, 1, 0, false, false);
                break;
            case SFX_Clip.WINNER_HORN_2_SFX:
                AudioCallBack2D(audioPlayer[ReturnPlayer()], audio_SFX[9], 1.5f, 0, 0, 1, 0, false, false);
                break;
            case SFX_Clip.End_Cheering_SFX:
                AudioCallBack2D(endCheeringSoruce, audio_SFX[10], .6f, 0, 0, 1, 0, false, false);
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
    private void AudioCallBack2D(AudioSource player, AudioClip clip, float volume, float fadeDuration, float pan, float pitch, float delayAudio, bool loop, bool fadeIn)
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
        for (int i = 0; i < audioPlayer.Length; i++)
        {
            if (!audioPlayer[i].isPlaying)
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


    //In game logic ---------------------------------------------------
    public void FilterOutAudio(bool isFilter)
    {
        if (isFilter)
        {
            audioMixer.DOSetFloat("FilterSound", 500, 1).SetEase(Ease.InOutSine);
        }
        else
        {
            audioMixer.DOSetFloat("FilterSound", 22000, .5f).SetEase(Ease.InOutSine);
        }
    }

    public void FadeOutAudio(bool isVolume)
    {
        if (isVolume)
        {
            audioMixer.DOSetFloat("SFX_Fader", -80, 1).SetEase(Ease.InOutSine);
        }
        else
        {
            audioMixer.DOSetFloat("SFX_Fader", 0, .3f).SetEase(Ease.InOutSine);
        }
    }
    public void StopStartWaterSFX(bool isStopped)
    {
        if (isStopped)
        {
            waterAtmoPlayer.Stop();
        }
        else
        {
            waterAtmoPlayer.Play();
        }
    }

    public void WinnerSoundEffect()
    {
        warpPipeAudioSource.clip = audio_SFX[4];
        warpPipeAudioSource.volume = 0f;
        warpPipeAudioSource.Play();
        warpPipeAudioSource.DOFade(.5f, 1).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            warpPipeAudioSource.DOFade(0, 1).SetDelay(6).SetEase(Ease.InOutSine);
        });
    }
}
