using UnityEngine;
using DG.Tweening;

public class Cheering_SFX : MonoBehaviour
{
    private AudioSource cheeringPlayer;
    private BoxCollider2D boxcollider2D;
    private bool playedOnce;
    private Tween fadeStopTween;

    private void Awake()
    {
        cheeringPlayer = GetComponent<AudioSource>();
        boxcollider2D = GetComponent<BoxCollider2D>();
        cheeringPlayer.volume = 0;
    }

    private void OnEnable()
    {
        GameManager.OnWinner += ZeroOut;
        GameManager.OnResetPlayers += SetClears;
        GameManager.OnClearPlayers += SetClears;
    }

    private void OnDisable()
    {
        GameManager.OnWinner -= ZeroOut;
        GameManager.OnResetPlayers -= SetClears;
        GameManager.OnClearPlayers -= SetClears;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!playedOnce)
        {
            cheeringPlayer.Stop();
            fadeStopTween?.Kill();
            if (collision)
            {
                cheeringPlayer.Play();
                fadeStopTween = cheeringPlayer.DOFade(.1f, 3);
                playedOnce = true;
            }
        }
    }

    private void ZeroOut()
    {
        fadeStopTween?.Kill();
        fadeStopTween = cheeringPlayer.DOFade(0, .1f);
        cheeringPlayer.Stop();
    }
    private void SetClears()
    {
        playedOnce = false;
    }


}
