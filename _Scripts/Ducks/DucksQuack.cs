using System.Collections;
using UnityEngine;

public class DucksQuack : MonoBehaviour
{
    public AudioClip[] quackSmaple;
    public AudioClip popSFX;
    public AudioClip splashSFX;
    public AudioSource quackMachine;
    public AudioSource splashMachine;
   
    private bool haveQuacked;
   

    public void Quack()
    {
        if(!haveQuacked)
        {
            haveQuacked = true;
            int clip = Random.Range(0, 2);
            quackMachine.pitch = 1;
            quackMachine.volume = .8f;
            quackMachine.clip = quackSmaple[clip];
            quackMachine.Play();
            StartCoroutine(TimerWait());
        }
    }
    public void ClearSFX()
    {
        quackMachine.pitch = 1;
        quackMachine.volume = .3f;
        quackMachine.clip = popSFX;
        quackMachine.Play();
    }

    public void SplashSFX()
    {
        splashMachine.pitch = 1;
        splashMachine.volume = .3f;
        splashMachine.clip = splashSFX;
        splashMachine.Play();
    }

    private IEnumerator TimerWait()
    {
        yield return new WaitForSeconds(5);
        haveQuacked = false;
    }
}
