using UnityEngine;



public class WaterSpeedTrap : MonoBehaviour
{
    private string[] tagsArray = { "SpeedUp", "SlowDown" };
    [SerializeField]private SpriteRenderer sprite;

    private void Awake()
    {
        sprite.GetComponent<SpriteRenderer>();  
    }

    private void Start()
    {
        int randoNumb = Random.Range(0, tagsArray.Length);
        gameObject.tag = tagsArray[randoNumb];
        WaterDirection(randoNumb);
    }

    private void WaterDirection(int direction)
    {
        //Start Animation
        if (direction != 0)
            sprite.flipX = true;
    }

}
