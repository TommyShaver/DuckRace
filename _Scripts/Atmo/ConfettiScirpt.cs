using System.Collections;
using UnityEngine;

public class ConfettiScirpt : MonoBehaviour
{
    private ParticleSystem confettiParticleSystem;
    private AudioSource audioSource;

    private void Awake()
    {
        confettiParticleSystem = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        GameManager.OnDucksGo += PlayAnimation;
    }
    private void OnDisable()
    {
        GameManager.OnDucksGo -= PlayAnimation;
    }

    private void PlayAnimation()
    {
        StartCoroutine(FireCommand());
    }

    private IEnumerator FireCommand()
    {
        yield return new WaitForSeconds(1);
         confettiParticleSystem.Play();
        if (audioSource != null)
        {
            audioSource.pitch = UnityEngine.Random.Range(1, 3);
            audioSource.volume = .3f;
            audioSource.PlayDelayed(UnityEngine.Random.Range(.1f, .4f));
        }
    }
    
}
