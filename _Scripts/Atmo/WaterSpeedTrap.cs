using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.CompilerServices;

public class WaterSpeedTrap : MonoBehaviour
{
    private string[] tagsArray = { "SpeedUp", "SlowDown" };
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator animationPath;
    [SerializeField] Color[] startingColors;
    private Tween waveScaling;
    private AudioSource audioSource;
    private bool higherPitch;
    private bool hasLayer;

    private void Awake()
    {
        sprite.GetComponent<SpriteRenderer>();
        animationPath.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        PlayField.OnDespawn += OnDeSpawn;
        PlayField.OnSpawnWaves += GetLayer;
    }

    private void OnDisable()
    {
        PlayField.OnDespawn -= OnDeSpawn;
        PlayField.OnSpawnWaves -= GetLayer;
    }

    private void Start()
    {
        int randoNumb = Random.Range(0, tagsArray.Length);
        gameObject.tag = tagsArray[randoNumb];
        sprite.color = startingColors[Random.Range(0, startingColors.Length)];
        animationPath.speed = Random.Range(0.2f,0.5f);
        if(randoNumb == 0)
        {
            higherPitch = true;
        }
        WaterDirection(randoNumb);
        OnSpawn();
        StartCoroutine(WaitSomeTime(true));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject)
        {
            if (higherPitch)
            {
                audioSource.pitch = 1.5f;
                audioSource.Play();
            }
            else
            {
                audioSource.pitch = .9f;
                audioSource.Play();
            }
        }
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

    private void GetLayer(int layer)
    {
        if(!hasLayer)
        {
            sprite.sortingOrder = layer;
            hasLayer = true;
        }
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
