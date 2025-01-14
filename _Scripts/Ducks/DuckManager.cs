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
    }
    private void OnDisable()
    {
        SpawnManager.OnSpawn -= GiveUsernameToDuck;
        UIManager.OnClearPlayers -= DestoryDuck;
        UIManager.OnResetPlayers -= ResetDuck;
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
    }

    public void SpeedChanged(bool changed)
    {
        if (changed)
        {
            duckFace.RandomFace();
        }
        else
        {
            duckFace.DefaultFace();
        }
    }
    public void DestoryDuck()
    {
        Destroy(gameObject);
    }
}
