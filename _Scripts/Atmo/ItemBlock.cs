using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;

public class ItemBlock : MonoBehaviour
{
    public ParticleSystem confettiParticles1;
    public ParticleSystem confettiParticles2;
    private SpriteRenderer spriteRenderer;
    private bool canInteract;
    private Tween bounceAnim;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        canInteract = true;
        int yPos = Mathf.RoundToInt(transform.position.y);
        SetLayer(yPos);
        OpeningAnim();
    }

    private void OpeningAnim()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(new Vector3(2, 2, 0), .5f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            bounceAnim = transform.DOLocalMoveY(transform.position.y + .1f, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        });
    }

    private void SetLayer(int pos)
    {
        int baseSortingOrder = 49;
        int offset = -pos;
        spriteRenderer.sortingOrder = baseSortingOrder + offset;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canInteract)
            return;

        DuckInterractionInterface hit = collision.GetComponent<DuckInterractionInterface>();
        if (hit != null)
        {
            hit.DuckInterraction("GotItem", true);
        }
        Color currentColor = spriteRenderer.color;
        currentColor.a = 0;
        spriteRenderer.color = currentColor;
        canInteract = false;
        confettiParticles1.Play();
        confettiParticles2.Play();
        StartCoroutine(RespawnBlock());

    }

    private IEnumerator RespawnBlock()
    {
        yield return new WaitForSeconds(3);
        Color currentColor = spriteRenderer.color;
        currentColor.a = 1;
        spriteRenderer.color = currentColor;
        OpeningAnim();
        canInteract = true;
    }

}
