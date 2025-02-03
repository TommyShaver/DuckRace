using UnityEngine;
using DG.Tweening;

public class WaterSpeedTrap : MonoBehaviour
{
    private string[] tagsArray = { "SpeedUp", "SlowDown" };
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator animationPath;
    [SerializeField] Color[] startingColors;
    private void Awake()
    {
        sprite.GetComponent<SpriteRenderer>();
        animationPath.GetComponent<Animator>();
    }

    private void Start()
    {
        int randoNumb = Random.Range(0, tagsArray.Length);
        gameObject.tag = tagsArray[randoNumb];
        sprite.color = startingColors[Random.Range(0, startingColors.Length)];
        animationPath.speed = Random.Range(0.2f,0.5f);
        WaterDirection(randoNumb);
        transform.DOScaleY(1.2f, RandomNumber()).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutBack);
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
}
