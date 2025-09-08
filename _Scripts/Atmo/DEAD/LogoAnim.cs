using UnityEngine;
using DG.Tweening;
using System.Collections;

public class LogoAnim : MonoBehaviour
{
    public GameObject logoAnim;
    private SpriteRenderer spriteRenderer;
    private AudioSource logoPlayer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        logoPlayer = GetComponent<AudioSource>();
    }

    void Start()
    {
        StartCoroutine(WaitPlease());
    }

    private void AnimationDoTween()
    {
        spriteRenderer.DOFade(1, 1).SetEase(Ease.OutQuad);
        logoPlayer.PlayDelayed(.5f);
        logoAnim.transform.DOLocalMoveX(-16f, 2).SetDelay(.7f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            spriteRenderer.DOFade(0, 1).SetEase(Ease.OutQuad);
        });

    }

    private IEnumerator WaitPlease()
    {
        yield return new WaitForSeconds(.5f);
        AnimationDoTween();
    }
}
