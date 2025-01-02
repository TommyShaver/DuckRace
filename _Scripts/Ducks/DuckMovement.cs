using System.Collections;
using UnityEngine;
using DG.Tweening;

public class DuckMovement : MonoBehaviour
{
    [SerializeField] Vector3 startPos;
    [SerializeField] float speedSet;
    [SerializeField] float storedSpeed;
    [SerializeField] bool slowDownCheck;
    [SerializeField] bool movementGo;

    //Set up---------------------------------------------
    void Start()
    {
        startPos = transform.position;
        SetSpeed();
        StartCoroutine(OnLoadAnim());
    }

    //Logic----------------------------------------------- 
    void Update()
    {
        if (movementGo)
        {
            transform.Translate(speedSet * Time.deltaTime * Vector2.right);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SlowDown")
        {
            SlowDownDuck();
        }
        if (collision.gameObject.tag == "SpeedUp")
        {
            SpeedUpDuck();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ResetSpeed();
    }

    //Speed And Postion setup (Incoming Info) ------------------------------
    public float SetSpeed()
    {
        float speed = Random.Range(1, 5);
        speedSet = speed;
        storedSpeed = speed;
        return speed;
    }

    public void SetPostion()
    {
        transform.position = startPos;
    }

    public void MovementStart()
    {
        movementGo = true;
    }
    public void MovementStop()
    {
        movementGo = false;
    }

    private void SlowDownDuck()
    {
        if (speedSet >= 3)
        {
            speedSet -= 2;
        }
        else 
        {
            speedSet -= .5f; 
        }
    }
    private void SpeedUpDuck()
    {
        speedSet += 3;
    }

    private void ResetSpeed()
    {
        speedSet = storedSpeed;
    }

    //on load
    private IEnumerator OnLoadAnim()
    {
        transform.DOMoveY(transform.position.y + 1, 0, true);
        yield return new WaitForSeconds(0.2f);
        transform.DOMoveY(startPos.y, .5f, false).SetEase(Ease.OutBounce);
        //send command landed in water
    }
}
