using System.Collections;
using UnityEngine;

public class DucksQuack : MonoBehaviour
{
    public AudioClip[] quackSmaple;
    public AudioClip splashSFX;
    public AudioClip rocketSFX;
    public AudioClip intertubeSpawnSFX;
    public AudioClip intertubeDespawnerSFX;
    public AudioClip getItemSFX;
    public AudioSource quackMachine;
    public AudioSource splashMachine;

    private bool haveQuacked;

    public enum SampledClips
    {
        quackSFX, SplashSFX, RocketSFX, IntertubeSpawnSFX, IntertubeDespawnSFX,
        quackSadSFX, quackHappySFX, getItemSFX   
    }

 

    public void DuckSFX(SampledClips clips)
    {
        switch (clips)
        {
            case SampledClips.quackSFX:
                if (!haveQuacked)
                {
                    haveQuacked = true;
                    StartCoroutine(TimerWait());
                    PlaySFX(quackSmaple[UnityEngine.Random.Range(0, quackSmaple.Length)], quackMachine, 1, 1f);
                }
                break;
            case SampledClips.quackHappySFX:
                PlaySFX(quackSmaple[UnityEngine.Random.Range(0, quackSmaple.Length)], quackMachine, 1.5f, 1f);
                break;
            case SampledClips.quackSadSFX:
                PlaySFX(quackSmaple[UnityEngine.Random.Range(0, quackSmaple.Length)], quackMachine, .8f, 1f);
                break;
            case SampledClips.SplashSFX:
                PlaySFX(splashSFX, splashMachine, 1, .4f);
                break;
            case SampledClips.RocketSFX:
                PlaySFX(rocketSFX, splashMachine, 1, .3f);
                break;
            case SampledClips.IntertubeSpawnSFX:
                PlaySFX(intertubeSpawnSFX, splashMachine, 1, .5f);
                break;
            case SampledClips.IntertubeDespawnSFX:
                PlaySFX(intertubeDespawnerSFX, splashMachine, 1, .5f);
                break;
            case SampledClips.getItemSFX:
                PlaySFX(getItemSFX, splashMachine, 1, .3f);
                break;
        }
    }

    private void PlaySFX(AudioClip clip, AudioSource source, float pitch, float volume)
    {
        source.clip = clip;
        source.pitch = pitch;
        source.volume = volume;
        source.Play();
    }

    private IEnumerator TimerWait()
    {
        yield return new WaitForSeconds(5);
        haveQuacked = false;
    }
}
