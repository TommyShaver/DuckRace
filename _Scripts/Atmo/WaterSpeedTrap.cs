using UnityEngine;



public class WaterSpeedTrap : MonoBehaviour
{
    private string[] tagsArray = { "SpeedUp", "SlowDown" };
    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite.GetComponent<SpriteRenderer>();  
    }

    private void Start()
    {
        int randoNumb = Random.Range(0, tagsArray.Length);
        gameObject.tag = tagsArray[randoNumb];
        WaterDirection(randoNumb);
        Debug.Log("Water Speed Trap" +  gameObject.tag);
    }

    private void WaterDirection(int direction)
    {
        if (direction == 0)
        {
            //Play srpite forward 
        }
        else
        {
            //Play Spirte other way
        }

    }

}
