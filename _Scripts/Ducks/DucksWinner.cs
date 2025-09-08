using UnityEngine;
using DG.Tweening;

public class DucksWinner : MonoBehaviour
{
    public SpriteRenderer holoDuck;
    public GameObject leftLightAnim;
    public GameObject rightLightAnim;
    public SpriteRenderer leftLightRenderer;
    public SpriteRenderer rightLightRenderer;

    private SpriteRenderer spriteRenderer;
    private DuckHats duckHat;
    private ParticleSystem winnerParticles;
    private AudioSource winnerSFX;
    private float startPosY;
    private float endPosY = -4f;
    private float cycDuration = 3.25f;
    private Tween colorTween;
    private Tween leftLightAnimTween;
    private Tween rightLightAnimTween;

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
        GameManager.OnResetPlayers += CleanUpWinner;
        GameManager.OnClearPlayers += CleanUpWinner;
    }

    private void OnDisable()
    {
        GameManager.OnWinAnimation -= ShowWinner;
        GameManager.OnWinAnimationCleanup -= CleanUpWinner;
        GameManager.OnResetPlayers -= CleanUpWinner;
        GameManager.OnClearPlayers -= CleanUpWinner;
    }

    private void Start()
    {
        startPosY = transform.position.y;
        leftLightRenderer.DOFade(0, .5f);
        rightLightRenderer.DOFade(0, .5f);
    }

    //? Winner called ............................................................
    private void ShowWinner(string hat, Color color, bool isRaindow, bool isHolo)
    {

        RainbowCall(isRaindow, color);

        HoloCall(isHolo);

        HatCall(hat);

        LightAnimation();
        transform.DOMoveY(endPosY, .5f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            winnerParticles.Play();
            winnerSFX.Play();
        });
    }

    private void RainbowCall(bool rainbow, Color color)
    {
        if (rainbow)
        {
            RainBowDuck();
        }
        else
        {
            spriteRenderer.color = color;
        }
    }

    private void HoloCall(bool holo)
    {
        if (holo)
        {
            holoDuck.enabled = true;
        }
        else
        {
            holoDuck.enabled = false;
        }
    }

    private void HatCall(string hat)
    {
        duckHat.ChangeHat(hat);
    }

    //Animation of Lights ___________________________________________________________
    private void LightAnimation()
    {
        float setTime = UnityEngine.Random.Range(1, 1.5f);
        float setDelay = UnityEngine.Random.Range(0.5f, 1);
        leftLightAnimTween = leftLightAnim.transform.DOLocalRotate(new Vector3(0, 0, 35), setTime).SetDelay(setDelay).SetEase(Ease.InOutBack).SetLoops(-1, LoopType.Yoyo);
        rightLightAnimTween = rightLightAnim.transform.DOLocalRotate(new Vector3(0, 0, -60), setTime).SetDelay(setDelay).SetEase(Ease.InOutBack).SetLoops(-1, LoopType.Yoyo);
        leftLightRenderer.DOFade(.3f, .5f);
        rightLightRenderer.DOFade(.3f, .5f);
    }


    //Clean up Everything -----------------------------------------------------------
    private void CleanUpWinner()
    {
        transform.DOMoveY(startPosY, 1).SetEase(Ease.InBack);
        colorTween.Kill();
        leftLightAnim.transform.localEulerAngles = new Vector3(0, 0, 28.8f);
        rightLightAnim.transform.localEulerAngles = new Vector3(0, 0, -50);
        leftLightAnimTween.Kill();
        rightLightAnimTween.Kill();
        leftLightRenderer.DOFade(0, .5f);
        rightLightRenderer.DOFade(0, .5f);
        duckHat.ChangeHat("none");
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
