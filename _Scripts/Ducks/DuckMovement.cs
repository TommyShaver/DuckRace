using System.Collections;
using UnityEngine;
using DG.Tweening;

public class DuckMovement : MonoBehaviour
{
    [SerializeField] Vector3 startPos;
    [SerializeField] float speedSet;
    [SerializeField] bool movementGo;
    private float storedSpeed;

    private bool stoptacking;
    private bool alreadyHappening; // so it doesn't trigger event twice.
    public bool canChangeSpeed;
    private DuckManager duckManager;

    private Tween roationTwen;
    private GameObject finishLine;

    //Set up---------------------------------------------
    private void Awake()
    {
        duckManager = GetComponent<DuckManager>();
    }

    void Start()
    {
        this.transform.Rotate(-15, 0, 0);
        startPos = transform.position;
        SetSpeed();
        StartCoroutine(OnLoadAnim());
        finishLine = GameObject.FindWithTag("FinishLine");
        stoptacking = false;
    }

    //Logic ----------------------------------------------- 
    void Update()
    {
        if (!movementGo)
            return;


        transform.Translate(speedSet * Time.deltaTime * Vector2.right);
        if (!stoptacking)
        {
            float distance = Vector3.Distance(gameObject.transform.position, finishLine.transform.position);
            duckManager.GetPostion(distance); // Send to duck manager to send to game manager to track postion for camera.
        }
    }

    //Speed And Postion setup (Incoming Info) ------------------------------
    public float SetSpeed()
    {
        float speed = UnityEngine.Random.Range(3.5f, 5f);
        storedSpeed = speed;
        speedSet = speed;
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
        canChangeSpeed = true;
    }
    public void MovementStop()
    {
        movementGo = false;
        stoptacking = false;
        roationTwen.Kill();
    }


    //Inbound commands from manager ------------------------------------------
    public void SlowDownDuck()
    {
        if (alreadyHappening)
            return;
        if (!canChangeSpeed)
            return;

        speedSet -= 2;
        alreadyHappening = true;
        GetSpeed(speedSet);
    }
    public void SpeedUpDuck()
    {
        if (!canChangeSpeed)
            return;
        speedSet += 2;
        GetSpeed(speedSet);
    }
    public void SpeedStop()
    {
        speedSet = 0;
        GetSpeed(speedSet);
    }

    public void RocketSpeed()
    {
        speedSet += 5;
        GetSpeed(speedSet);
    }

    public void TauntSpeed()
    {
        speedSet = 1;
        GetSpeed(speedSet);
    }

    public void ResetSpeed()
    {
        if (!canChangeSpeed)
            return;
        speedSet = storedSpeed;
        GetSpeed(speedSet);
        alreadyHappening = false;
    }

    public void ChangeYPostion(int i)
    {
        //Game not started get out of it.
        if (!movementGo) return;

        transform.DOMoveY(transform.position.y + i, .5f).SetEase(Ease.InOutBack);
    }

    public void SlowDownAfterFinishLineCross()
    {
        ResetSpeed();
        StartCoroutine(SlowDownOverTime());
    }
    public void CleanUP()
    {
        StopAllCoroutines();
    }


    //Outbound commnads --------------------------------------------------------------
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
        transform.DOMoveY(startPos.y, .5f, false).SetEase(Ease.OutBounce);
        duckManager.DuckTouchedWater();
    }

    private IEnumerator SlowDownOverTime()
    {
        while (0 <= speedSet)
        {
            yield return new WaitForSeconds(0.2f);
            speedSet -= 0.5f;
        }
        SpeedStop();
    }
}
