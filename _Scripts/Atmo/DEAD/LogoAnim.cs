using UnityEngine;
using DG.Tweening;
using System.Collections;

public class LogoAnim : MonoBehaviour
{
    //public GameObject logoAnim;
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
        
    }
    private IEnumerator WaitPlease()
    {
        yield return new WaitForSeconds(.5f);
        AnimationDoTween();
    }
}
