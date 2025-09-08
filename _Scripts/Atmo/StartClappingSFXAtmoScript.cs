using UnityEngine;
using DG.Tweening;

public class StartClappingSFXAtmoScript : MonoBehaviour
{
    private AudioSource audioSource;
    private Tween stopFade;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        GameManager.OnDucksGo += GoSFX;
        GameManager.OnResetPlayers += CleanUPGoSoundEffects;
        GameManager.OnClearPlayers += CleanUPGoSoundEffects;
    }

    private void OnDisable()
    {
        GameManager.OnDucksGo -= GoSFX;
        GameManager.OnResetPlayers += CleanUPGoSoundEffects;
        GameManager.OnClearPlayers += CleanUPGoSoundEffects;
    }

    private void GoSFX()
    {
        stopFade.Kill();
        audioSource.volume = 0.7f;
        audioSource.PlayDelayed(1);
    }

    private void CleanUPGoSoundEffects()
    {
        stopFade = audioSource.DOFade(0, .5f).OnComplete(() =>
        {
            audioSource.Stop();
        });

    }
}
