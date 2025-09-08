using UnityEngine;
using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;

public class DuckEffectsRocket : MonoBehaviour
{
    public Color[] rocketColorEffects;
    private ParticleSystem smokeEffect;
    private ParticleSystemRenderer sortingOrder;
    private SpriteRenderer spriteRenderer;
    private Color startingColor;
    private bool colorAnim;
    private int currentLayer = 51;
    private int defaultLayer;
    private Tween bounceTween;
    private Coroutine flamesAnim;

    private void Awake()
    {
        smokeEffect = GetComponentInChildren<ParticleSystem>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        sortingOrder = smokeEffect.GetComponentInChildren<ParticleSystemRenderer>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingColor = spriteRenderer.color;
        startingColor.a = 0;
        spriteRenderer.color = startingColor;
        smokeEffect.Stop();
    }

    public void RocketEffectStart()
    {
        spriteRenderer.DOFade(1, .2f).OnComplete(() =>
        {
            colorAnim = true;
            flamesAnim = StartCoroutine(ColorSwitcher());
            smokeEffect.Play();
            bounceTween = transform.DOLocalMoveY(-.23f, .2f).SetEase(Ease.InOutBounce).SetLoops(-1, LoopType.Yoyo);
        });
    }

    public void RocketEffectStop()
    {
        colorAnim = false;
        spriteRenderer.DOFade(0, .2f);
        smokeEffect.Stop();
        bounceTween.Kill();
        StopAllCoroutines();
    }



    public void SpawnEffectItems(int i)
    {
        sortingOrder.sortingOrder = currentLayer + i;
        spriteRenderer.sortingOrder = currentLayer + i;
        defaultLayer = currentLayer + i;
    }

    public void DuckChangedLayer(int i)
    {
        sortingOrder.sortingOrder += i;
        spriteRenderer.sortingOrder += i;
    }

    public void SetDefaultLayer()
    {
        spriteRenderer.sortingOrder = defaultLayer;
        sortingOrder.sortingOrder = defaultLayer;
    }
    
    private IEnumerator ColorSwitcher()
    {
        while (colorAnim)
        {
            for (int i = 0; i < rocketColorEffects.Length; i++)
            {
                spriteRenderer.color = rocketColorEffects[i];
                yield return new WaitForSeconds(.1f);
            }
        }
    }
}
