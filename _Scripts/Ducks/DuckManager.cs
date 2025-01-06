using UnityEngine;

public class DuckManager : MonoBehaviour
{
    private DuckMovement duckMovement;
    private DuckTextUserName duckText;
    private DuckColor duckColor;

    //Setup ---------------------------------------------------------------
    private void Awake()
    {
        duckMovement = GetComponent<DuckMovement>();
        duckText = GetComponentInChildren<DuckTextUserName>();
        duckColor = GetComponentInChildren<DuckColor>();
    }
    private void OnEnable()
    {
        SpawnManager.OnSpawn += GiveUsernameToDuck;
    }
    private void OnDisable()
    {
        SpawnManager.OnSpawn -= GiveUsernameToDuck;
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
}
