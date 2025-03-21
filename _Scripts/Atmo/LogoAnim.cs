using UnityEngine;
using DG.Tweening;

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
        AnimationDoTween();
    }

    private void AnimationDoTween()
    {
        spriteRenderer.DOFade(1, 1).SetEase(Ease.OutQuad);
        logoPlayer.PlayDelayed(1);
        logoAnim.transform.DOLocalMoveX(-16f, 2).SetDelay(.7f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            spriteRenderer.DOFade(0, 1).SetEase(Ease.OutQuad);
        });

    }
}
