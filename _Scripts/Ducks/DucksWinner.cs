using UnityEngine;
using DG.Tweening;

public class DucksWinner : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private DuckHats duckHat;
    private ParticleSystem winnerParticles;
    private AudioSource winnerSFX;
    private float startPosY;
    private float endPosY = -4f;
    private float cycDuration = 3.25f;
    private Tween colorTween;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        duckHat = GetComponentInChildren<DuckHats>();
        winnerParticles = GetComponentInChildren<ParticleSystem>();
        winnerSFX = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        GameManager.OnWinAnimation += ShowWinner;
        GameManager.OnWinAnimationCleanup += CleanUpWinner;
    }

    private void OnDisable()
    {
        GameManager.OnWinAnimation -= ShowWinner;
        GameManager.OnWinAnimationCleanup -= CleanUpWinner;
    }

    private void Start()
    {
        startPosY = transform.position.y;
    }

    private void ShowWinner(string hat, Color color, bool isRaindow)
    {
        if (isRaindow)
        {
            RainBowDuck();
        }
        else
        {
            spriteRenderer.color = color;
        }
        duckHat.ChangeHat(hat);
        transform.DOMoveY(endPosY, .5f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            winnerParticles.Play();
            winnerSFX.Play();
        });
    }

    private void CleanUpWinner()
    {
        transform.DOMoveY(startPosY, 1).SetEase(Ease.InBack);
        colorTween.Kill();
    }

    private void RainBowDuck()
    {
        colorTween = DOTween.To(() => 0f, h =>
        {
            Color color = Color.HSVToRGB(h, 1f, 1f);
            spriteRenderer.color = color;
        }, 1f, cycDuration).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }
}
