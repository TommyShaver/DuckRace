using UnityEngine;

public class DuckManager : MonoBehaviour
{
    private DuckMovement duckMovement;
    private DuckTextUserName duckText;
    private DuckColor duckColor;
    private DuckFace duckFace;

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
        UIManager.OnClearPlayers += DestoryDuck;
        UIManager.OnResetPlayers += ResetDuck;
        UIManager.OnDucksGo += MovementStart;
    }
    private void OnDisable()
    {
        SpawnManager.OnSpawn -= GiveUsernameToDuck;
        UIManager.OnClearPlayers -= DestoryDuck;
        UIManager.OnResetPlayers -= ResetDuck;
        UIManager.OnDucksGo -= MovementStart;
    }

    //Logic --------------------------------------------------------------------
    public void ResetDuck()
    {
        duckMovement.MovementStop();
        duckMovement.SetPostion();
        duckMovement.SetSpeed();
    }

    public void GiveUsernameToDuck(string username, int layer)
    {
        duckText.NameTag(username, layer);
        duckColor.ChangeSortingLayer(layer);
        Debug.Log(username + " " +  layer + " <- username DuckManager");
    }

    public void MovementStart()
    {
        duckMovement.MovementStart();
        duckColor.SpriteRoation(true); // I had to do it this way to not change the directoin of gameobject. or I would just leave this on the gameobject.
    }

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
    public void DestoryDuck()
    {
        duckColor.KillTween();
        Destroy(gameObject);
    }
}
