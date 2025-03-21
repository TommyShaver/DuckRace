using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;
using Unity.VisualScripting;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    public AudioClip[] audio_SFX;
    public AudioSource[] audioPlayer;
    public AudioMixer audioMixer;

    public BoxCollider2D SFX_Triggers;

    public bool playedOnce;
    public bool serectSongPlaing;
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
        AtmoWater_SFX();
        audioPlayer[5].volume = 0;
    }

    //Sound Player 0 ------------------------------------------------------
    private void AtmoWater_SFX()
    {
        audioPlayer[0].volume = .7f;
        audioPlayer[0].pitch = .7f;
        audioPlayer[0].clip = audio_SFX[0];
        audioPlayer[0].Play();
    }

    //Sound Player 1 -----------------------------------------------------
    public void SpawnSpalsh_SFX(bool isDuck)
    {
        if (isDuck)
        {
            float randomPitch = Random.Range(1.1f, 1.8f);
            audioPlayer[1].pitch = randomPitch;
            audioPlayer[1].panStereo = -.5f;
            audioPlayer[1].volume = .4f;
            audioPlayer[1].clip = audio_SFX[1];
            audioPlayer[1].Play();
        }
        else
        {
            audioPlayer[1].pitch = 1;
            audioPlayer[1].panStereo = 0;
            audioPlayer[1].volume = .6f;
            audioPlayer[1].clip = audio_SFX[1];
            audioPlayer[1].Play();
        }
    }
    public void DeSpwanSpalsh_SFX()
    {
        audioPlayer[1].pitch = 1;
        audioPlayer[1].panStereo = 0;
        audioPlayer[1].volume = 1f;
        audioPlayer[1].clip = audio_SFX[2];
        audioPlayer[1].Play();
    }

    //Sound Player 2 ---------------------------------------------------------
    public void Duck_SFX()
    {
        int clip = Random.Range(3, 5);
        audioPlayer[2].pitch = 1;
        audioPlayer[2].volume = .7f;
        audioPlayer[2].clip = audio_SFX[clip];
        audioPlayer[2].Play();
    }

    //Sound Player 3 ----------------------------------------------------------
    public void ClearPlayers_SFX()
    {
        audioPlayer[3].clip = audio_SFX[6];
        audioPlayer[3].Play();
    }
    public void StartGame_SFX()
    {
        audioPlayer[3].clip = audio_SFX[12];
        audioPlayer[3].Play();
    }
    public void Horn_SFX(int clip)
    {
        audioPlayer[3].clip = audio_SFX[clip];
        audioPlayer[3].volume = 1;
        audioPlayer[3].Play();
    }

    public void SerectSong()
    {
        if (!serectSongPlaing)
        {
            audioPlayer[3].clip = audio_SFX[13];
            audioPlayer[3].volume = 1;
            audioPlayer[3].Play();
            audioMixer.DOSetFloat("Music_Fader", -80, 0.2f);
            StartCoroutine(WaitForSerect());
            serectSongPlaing = true;
        }
    }
    //Sound Player 4 ------------------------------------------------------------
    public void UIHover_Button_SFX(float pan)
    {
        audioPlayer[4].clip = audio_SFX[7];
        audioPlayer[4].volume = .5f;
        audioPlayer[4].panStereo = pan;
        audioPlayer[4].Play();
    }
    public void UISelect_Button_SFX()
    {
        audioPlayer[4].clip = audio_SFX[8];
        audioPlayer[4].volume = 1;
        audioPlayer[4].Play();
    }

    //Sound Effect Triggers --------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!playedOnce)
        {
            if (collision)
            {
                audioPlayer[5].clip = audio_SFX[14];
                audioPlayer[5].DOFade(.1f, 5);
                audioPlayer[5].Play();
            }
            playedOnce = true;
        }
    }
    
    public void EndGameCheeringPlayer()
    {
        audioPlayer[5].DOFade(0, .1f);
        audioPlayer[5].Stop();
    }

    //Sound Mixer Menu ---------------------------------------------------------
    public void AudioMixerSetting(float volume)
    {
        if(volume <= 0)
        {
            audioMixer.SetFloat("Volume_SFX", -80);
            return;
        }
        audioMixer.SetFloat("Volume_SFX", Mathf.Log10(volume) * 20);
    }
    private IEnumerator WaitForSerect()
    {
        yield return new WaitForSeconds(9.7f);
        audioMixer.DOSetFloat("Music_Fader", 0, 2);
        serectSongPlaing = false;
    }


}
