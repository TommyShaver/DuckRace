using Unity.VisualScripting;
using UnityEngine;

public class DuckManager : MonoBehaviour
{
    //Grab other Scripts
    private DuckMovement duckMovement;
    private DuckTextUserName duckText;
    private DuckColor duckColor;
    private DuckFace duckFace;

    //Incoming local var
    private float postionFromGoal;
    private float changedSpeed;

    //Setup ---------------------------------------------------------------
    private void Awake()
    {
        duckMovement = GetComponent<DuckMovement>();
        duckText = GetComponentInChildren<DuckTextUserName>();
        duckColor = GetComponentInChildren<DuckColor>();
        duckFace = GetComponentInChildren<DuckFace>();
    }
    private void OnEnable()
    {
        SpawnManager.OnSpawn += GiveUsernameToDuck;
        GameManager.OnClearPlayers += DestoryDuck;
        GameManager.OnResetPlayers += ResetDuck;
        GameManager.OnDucksGo += MovementStart;
        GameManager.OnStopDucks += DucksStop;
        GameManager.OnGrabLocation += SendPostionTOGameManger;
    }
    private void OnDisable()
    {
        SpawnManager.OnSpawn -= GiveUsernameToDuck;
        GameManager.OnClearPlayers -= DestoryDuck;
        GameManager.OnResetPlayers -= ResetDuck;
        GameManager.OnDucksGo -= MovementStart;
        GameManager.OnStopDucks -= DucksStop;
        GameManager.OnGrabLocation -= SendPostionTOGameManger;
    }

    //Spawn ---------------------------------------------------------------
    public void GiveUsernameToDuck(string username, int layer)
    {
        duckText.NameTag(username, layer);
        duckColor.ChangeSortingLayer(layer);
        Debug.Log(username + " " +  layer + " <- username DuckManager");
 
    }

    //Start Game ----------------------------------------------------------
    public void MovementStart()
    {
        duckMovement.MovementStart();
        duckColor.SpriteRoation(true); // I had to do it this way to not change the directoin of gameobject. or I would just leave this on the gameobject.
    }


    //Out Bound Logic ------------------------------------------------------
    public void GetPostion(float distance)
    {
        postionFromGoal = distance;
    }
    public void GetSpeed(float speed)
    {
        changedSpeed = speed;
        SendSpeedToGameManager();
    }

    //Inner Logic -------------------------------------------------------
    public void SpeedChanged(bool changed, bool isHappy)
    {
        if (changed)
        {
            duckFace.RandomFace(isHappy);
        }
        else
        {
            duckFace.DefaultFace();
        }
    }

    
    public void CrossedFinishLine()
    {
        GameManager.instance.DucksCrossedFinishLine(duckText.GetName());
    }

    private void DucksStop()
    {
        duckMovement.GameOver();
        duckColor.KillTween();
    }

    //OutBoundCalls ----------------------------------------------------
    private void SendPostionTOGameManger()
    {
        GameManager.instance.WhoIsFisrt(duckText.GetName(), postionFromGoal);
    }
    private void SendSpeedToGameManager()
    {
        GameManager.instance.DucksSpeed(duckText.GetName(), changedSpeed);
    }

    //Clean up ---------------------------------------------------------
    public void ResetDuck()
    {
        duckMovement.MovementStop();
        duckMovement.SetPostion();
        duckMovement.SetSpeed();
        duckColor.KillTween();
        postionFromGoal = 0;
    }

    public void DestoryDuck()
    {
        duckColor.KillTween();
        Destroy(gameObject);
    }

}
