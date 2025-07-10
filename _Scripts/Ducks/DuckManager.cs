using System.Collections;
using UnityEngine;

public class DuckManager : MonoBehaviour
{
    //Grab other Scripts
    private DuckMovement duckMovement;
    private DuckTextUserName duckText;
    private DuckColor duckColor;
    private DuckFace duckFace;
    private DucksParticalSystem duckEffects;
    private DuckHats duckHats;
    private DucksQuack duckSound;

    //Incoming local var
    private float postionFromGoal;
    private float changedSpeed;

    private string myName;
    private bool iHaveAName;

    //Setup ---------------------------------------------------------------
    private void Awake()
    {
        duckMovement = GetComponent<DuckMovement>();
        duckText = GetComponentInChildren<DuckTextUserName>();
        duckColor = GetComponentInChildren<DuckColor>();
        duckFace = GetComponentInChildren<DuckFace>();
        duckEffects = GetComponentInChildren<DucksParticalSystem>();
        duckHats = GetComponentInChildren<DuckHats>();
        duckSound = GetComponentInChildren<DucksQuack>();
    }
    private void OnEnable()
    {
        SpawnManager.OnSpawn += GiveUsernameToDuck;
        GameManager.OnClearPlayers += DestoryDuck;
        GameManager.OnResetPlayers += ResetDuck;
        GameManager.OnDucksGo += MovementStart;
        GameManager.OnStopDucks += DucksStop;
        GameManager.OnGrabLocation += SendPostionTOGameManger;

        //Twitch commands
        TwitchEventListner.OnColorChangeChat += ChangeColorRequat;
        TwitchEventListner.OnHatChangeChat += ChangeHatRequst;
        TwitchEventListner.OnDuckYChange += ChangeYRequst;
        TwitchEventListner.OnDuckQuack += PlayQuackDuck;
        TwitchEventListner.OnDuckTaunt += PlayTauntDuck;
        TwitchEventListner.OnDuckItemUse += ItemRequestUse;
        TwitchEventListner.OnBanPlayer += BanPlayer;
        TwitchEventListner.OnClearPlayer += ClearPlayer;
    }
    private void OnDisable()
    {
        SpawnManager.OnSpawn -= GiveUsernameToDuck;
        GameManager.OnClearPlayers -= DestoryDuck;
        GameManager.OnResetPlayers -= ResetDuck;
        GameManager.OnDucksGo -= MovementStart;
        GameManager.OnStopDucks -= DucksStop;
        GameManager.OnGrabLocation -= SendPostionTOGameManger;

        //Twitch request
        TwitchEventListner.OnColorChangeChat -= ChangeColorRequat;
        TwitchEventListner.OnHatChangeChat -= ChangeHatRequst;
        TwitchEventListner.OnDuckYChange -= ChangeYRequst;
        TwitchEventListner.OnDuckQuack -= PlayQuackDuck;
        TwitchEventListner.OnDuckTaunt -= PlayTauntDuck;
        TwitchEventListner.OnDuckItemUse -= ItemRequestUse;
        TwitchEventListner.OnBanPlayer -= BanPlayer;
        TwitchEventListner.OnClearPlayer -= ClearPlayer;
    }

    //Spawn ---------------------------------------------------------------
    public void GiveUsernameToDuck(string username, int layer)
    {
        if (!iHaveAName)
        {
            myName = username;
            iHaveAName = true;
            duckText.NameTag(username, layer);
            duckColor.ChangeSortingLayer(layer);
            duckHats.ChangeSortingLayer(layer);
            Debug.Log(username + " " + layer + " <- username DuckManager");
            duckEffects.SpawnParticleSystem(layer);
            duckFace.FaceLayer(layer);
            duckSound.Quack();
        }
    }

    public void DuckTouchedWater()
    {
        duckSound.SplashSFX();
        duckEffects.DuckLandInWater();
    }






    //Start Game ----------------------------------------------------------
    public void MovementStart()
    {
        duckMovement.MovementStart();
        duckColor.SpriteRoation(true); // I had to do it this way to not change the directoin of gameobject. or I would just leave this on the gameobject.
        duckEffects.EffectStart();
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
    private void SendPostionTOGameManger()
    {
        //GameManager.instance.WhoIsFisrt(duckText.GetName(), postionFromGoal);
    }
    private void SendSpeedToGameManager()
    {
        // GameManager.instance.DucksSpeed(duckText.GetName(), changedSpeed);
    }








    //Incoming Logic ====================================================
    //These commands are fired from Events from TwitchEvnetListener ----------
    private void ChangeColorRequat(string user, string portpy, string color)
    {
        if (user != myName) return;

        duckColor.ChatMemberChangedColor(color);
    }

    private void ChangeHatRequst(string user, string propty, string hat)
    {
        if (user != myName) return;

        duckHats.ChangeHat(hat);
    }

    private void ChangeYRequst(string user, string movement)
    {
        if (user != myName) return;

        int direction = 0;
        float currentY = transform.position.y;
        switch (movement)
        {
            case "up" when currentY != 1:
                direction = 1;
                break;
            case "down" when currentY != -5:
                direction = -1;
                break;
        }

        if(direction != 0)
        {
            duckText.DuckChangedLayer(direction);
            duckFace.DuckChangedLayer(direction);
            duckEffects.DuckChangedLayer(direction);
            duckHats.DuckChangedLayer(direction);
            duckColor.DuckChangedLayer(direction);
            duckMovement.ChangeYPostion(direction);
        }
    }

    private void PlayQuackDuck(string user)
    {
        if (user != myName) return;

        duckSound.Quack();
    }
    private void PlayTauntDuck(string user)
    {
        if (user != myName) return;

        //Play taunt effect
    }

    private void ItemRequestUse(string user)
    {
        if (user != myName) return;

        //Use items
    }
    private void BanPlayer(string user)
    {
        if (user != myName) return;
        Debug.Log("why did i ban a player?");
        SaveDataManager.instance.BanPlayerFromJoining(user);
        DestoryDuck();
    }

    private void ClearPlayer(string user)
    {
        if (user != myName) return;
        Debug.Log("Cleared player");
        SpawnManager.Instance.ClearPlayerFromSpot(user);
        DestoryDuck();
    }






    //transfer data between local srcipts -------------------------------
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
        //GameManager.instance.DucksCrossedFinishLine(duckText.GetName());
    }

    private void DucksStop()
    {
        duckMovement.GameOver();
        duckColor.KillTween();
    }


   

    //Clean up ---------------------------------------------------------
    public void ResetDuck()
    {
        duckMovement.MovementStop();
        duckMovement.SetPostion();
        duckMovement.SetSpeed();
        duckColor.KillTween();
        postionFromGoal = 0;
        duckEffects.EffectStop();
    }

    public void DestoryDuck()
    {
        duckColor.KillTween();
        duckSound.ClearSFX();
        Destroy(gameObject, 1);
    }
}
