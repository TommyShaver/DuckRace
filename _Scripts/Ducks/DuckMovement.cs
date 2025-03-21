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

    private bool stoptacking;
    private bool alreadyHappening; // so it doesn't trigger event twice.
    private DuckManager duckManager;

    private Tween roationTwen;
    private bool isHappy;
    private GameObject finishLine;

    //Set up---------------------------------------------
    private void Awake()
    {
        duckManager = GetComponent<DuckManager>();
    }

    void Start()
    {
        startPos = transform.position;
        SetSpeed();
        StartCoroutine(OnLoadAnim());
        finishLine = GameObject.FindWithTag("FinishLine");
        stoptacking = false;
    }

    //Logic ----------------------------------------------- 
    void Update()
    {
        if (movementGo)
        {
            transform.Translate(speedSet * Time.deltaTime * Vector2.right);
            if(!stoptacking)
            {
                float distance = Vector3.Distance(gameObject.transform.position, finishLine.transform.position);
                duckManager.GetPostion(distance); // Send to duck manager to send to game manager to track postion for camera.
            }
        }
    }

    //Trigger dection logic ---------------------------------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Please switch this to an interface
        if (collision.gameObject.tag == "SlowDown")
        {
            SlowDownDuck();
            isHappy = false;
        }
        if (collision.gameObject.tag == "SpeedUp")
        {
            SpeedUpDuck();
            isHappy = true;
        }
        if(collision.gameObject.tag == "FinishLine")
        {
            duckManager.CrossedFinishLine();
            stoptacking = true;
        }
        duckManager.SpeedChanged(true, isHappy);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ResetSpeed();
        duckManager.SpeedChanged(false, false);
        alreadyHappening = false;   
    }



    //Speed And Postion setup (Incoming Info) ------------------------------
    public float SetSpeed()
    {
        float speed = Random.Range(1.5f, 3.5f);
        speedSet = speed;
        storedSpeed = speed;
        GetSpeed(speed);
        return speed;
    }

    public void SetPostion()
    {
        transform.position = startPos;
    }

    //Movement logic --------------------------------------------------------
    public void MovementStart()
    {
        movementGo = true;
    }
    public void MovementStop()
    {
        movementGo = false;
        stoptacking = false;
        roationTwen.Kill();
    }

    private void SlowDownDuck()
    {
        if (!alreadyHappening)
        {
            if (speedSet > 3)
            {
                speedSet -= 3;
            }
            else
            {
                speedSet = .5f;
            }
            alreadyHappening = true;
            GetSpeed(speedSet);
        }
    }
    private void SpeedUpDuck()
    {
        speedSet += 3;
        GetSpeed(speedSet);
    }

    private void ResetSpeed()
    {
        speedSet = storedSpeed;
        GetSpeed(storedSpeed);
    }
    private void GetSpeed(float speedToDuckManager)
    {
        duckManager.GetSpeed(speedToDuckManager);
    }


    //Game Over --------------------------------------------------------------------------
    public void GameOver()
    {
        speedSet = 0;
        stoptacking = false;
    }
    
    

    //on load timer -----------------------------------------------------------------------
    private IEnumerator OnLoadAnim()
    {
        transform.DOMoveY(transform.position.y + 1, 0, true);
        yield return new WaitForSeconds(0.2f);
        SoundManager.instance.SpawnSpalsh_SFX(true);
        transform.DOMoveY(startPos.y, .5f, false).SetEase(Ease.OutBounce);
        //send command landed in water
    }
}
