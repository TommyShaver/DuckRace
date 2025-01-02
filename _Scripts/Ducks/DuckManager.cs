using UnityEngine;

public class DuckManager : MonoBehaviour
{
    private DuckMovement duckMovement;
    [SerializeField] private DuckTextUserName duckText;

    //Setup ---------------------------------------------------------------
    private void Awake()
    {
        duckMovement = GetComponent<DuckMovement>();
        duckText = GetComponentInChildren<DuckTextUserName>();
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

    public void GiveUsernameToDuck(string username)
    {
        duckText.NameTag(username);
        Debug.Log(username + " <- username DuckManager");
    }

    public void MovementStart()
    {
        duckMovement.MovementStart();
    }
}
