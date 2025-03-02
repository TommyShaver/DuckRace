using UnityEngine;
using DG.Tweening;

public class LogoAnim : MonoBehaviour
{
    public GameObject logoAnim;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        AnimationDoTween();
    }

    private void AnimationDoTween()
    {
        spriteRenderer.DOFade(1, 1).SetEase(Ease.OutQuad);
        logoAnim.transform.DOLocalMoveX(-16f, 2).SetDelay(.7f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            spriteRenderer.DOFade(0, 1).SetEase(Ease.OutQuad);
        });

    }
}
