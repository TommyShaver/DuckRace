using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;

public class WaterSpeedTrap : MonoBehaviour
{
    private string[] tagsArray = { "SpeedUp", "SlowDown" };
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator animationPath;
    [SerializeField] Color[] startingColors;
    private Tween waveScaling;

    private void Awake()
    {
        sprite.GetComponent<SpriteRenderer>();
        animationPath.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        PlayField.OnDespawn += OnDeSpawn;
    }

    private void OnDisable()
    {
        PlayField.OnDespawn -= OnDeSpawn;
    }

    private void Start()
    {
        int randoNumb = Random.Range(0, tagsArray.Length);
        gameObject.tag = tagsArray[randoNumb];
        sprite.color = startingColors[Random.Range(0, startingColors.Length)];
        animationPath.speed = Random.Range(0.2f,0.5f);
        WaterDirection(randoNumb);
        OnSpawn();
        StartCoroutine(WaitSomeTime(true));
    }

    private void WaterDirection(int direction)
    {
        //Start Animation
        if (direction == 0)
            sprite.flipX = true;
    }

    private int RandomNumber()
    {
        int randomNumber = Random.Range(1, 5);
        return randomNumber;
    }

   
    private void OnSpawn()
    {
        transform.DOScaleY(1.0138f, .5f).SetEase(Ease.OutExpo);
    }

    private void OnDeSpawn()
    {
        waveScaling.Kill();
        transform.DOScaleY(0, .5f).SetEase(Ease.OutExpo);
        StartCoroutine(WaitSomeTime(false));

    }

    private IEnumerator WaitSomeTime(bool scale)
    {
        yield return new WaitForSeconds(.9f);
        if(scale)
        {
           waveScaling = transform.DOScaleY(1.2f, RandomNumber()).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutBack);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
