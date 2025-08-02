using System.Collections;
using UnityEngine;

public class DuckManager : MonoBehaviour, DuckInterractionInterface, ICrossGoal
{

    //Grab other Scripts
    private DuckMovement duckMovement;
    private DuckTextUserName duckText;
    private DuckColor duckColor;
    private DuckFace duckFace;
    private DucksParticalSystem duckEffects;
    private DuckHats duckHats;
    private DucksQuack duckSound;
    private DuckItems duckItems;
    private DucksNoise ducksNoise;

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
        duckItems = GetComponentInChildren<DuckItems>();
        ducksNoise = GetComponentInChildren<DucksNoise>();
        
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
            duckSound.DuckSFX(DucksQuack.SampledClips.quackSFX);
            duckItems.SpawnDuckItems(layer);
            ducksNoise.ChangeSortingLayer(layer);
        }
    }



    public void DuckTouchedWater()
    {
        duckSound.DuckSFX(DucksQuack.SampledClips.SplashSFX);
        duckEffects.DuckLandInWater();
    }






    //Start Game ----------------------------------------------------------
    public void MovementStart()
    {
        duckMovement.MovementStart();
        duckColor.SpriteRoation(); // I had to do it this way to not change the directoin of gameobject. or I would just leave this on the gameobject.
        duckEffects.EffectStart();
    }








    //Call to GameManager Logic ------------------------------------------------------
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
        GameManager.instance.WhoIsFisrt(duckText.GetName(), postionFromGoal);
    }





    private void SendSpeedToGameManager()
    {
        GameManager.instance.DucksSpeed(duckText.GetName(), changedSpeed);
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
                ChangeLayerOfDuck(-1);
                break;
            case "down" when currentY != -5:
                direction = -1;
                ChangeLayerOfDuck(1);
                break;
        }

        if (direction != 0)
        {
            duckMovement.ChangeYPostion(direction);
        }
    }


    private void ChangeLayerOfDuck(int layer)
    {
        duckText.DuckChangedLayer(layer);
        duckFace.DuckChangedLayer(layer);
        duckEffects.DuckChangedLayer(layer);
        duckHats.DuckChangedLayer(layer);
        duckColor.DuckChangedLayer(layer);
        duckItems.DuckChangedLayer(layer);
        ducksNoise.DuckChangedLayer(layer);
    }




    private void PlayQuackDuck(string user)
    {
        if (user != myName) return;

        duckSound.DuckSFX(DucksQuack.SampledClips.quackSFX);
    }




    private void PlayTauntDuck(string user)
    {
        if (user != myName) return;

        duckColor.AnimTaunt();
        duckMovement.TauntSpeed();
        ducksNoise.FlipSprite(false);
        duckFace.FlipSprite(false);
    }





    public void GetOutOfTauntAnim()
    {
        duckMovement.ResetSpeed();
        ducksNoise.FlipSprite(true);
        duckFace.FlipSprite(true);
    }


    public void UseRocket()
    {
        duckSound.DuckSFX(DucksQuack.SampledClips.quackHappySFX);
        duckMovement.canChangeSpeed = false;
        duckMovement.RocketSpeed();
        duckFace.RandomFace(false);
        duckColor.RocketAnim();
    }

    public void GetOutOfRocket()
    {
        duckMovement.canChangeSpeed = true;
        duckMovement.ResetSpeed();
        duckFace.DefaultFace();
        duckColor.ReturnOutOfAnimation();
    }


    private void ItemRequestUse(string user)
    {
        if (user != myName) return;
        duckItems.UseItem();
        Debug.Log("Item Used Fucntion Trigger");

    }





    //! GET THIS GUY OUT OF HERE.................................
    private void BanPlayer(string user)
    {
        if (user != myName) return;
        SaveDataManager.instance.BanPlayerFromJoining(user);
        DestoryDuck();
    }

    private void ClearPlayer(string user)
    {
        if (user != myName) return;
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
        GameManager.instance.DucksCrossedFinishLine(duckText.GetName());
        duckMovement.SlowDownAfterFinishLineCross();
        duckEffects.EffectStop();
        duckColor.KillTween();
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
        duckItems.ResetCleanUp();
        duckMovement.CleanUP();  
    }

    public void DestoryDuck()
    {
        duckColor.KillTween();
        Destroy(gameObject, .5f);
    }











    //Interface --------------------------------------------------------
    public void DuckInterraction(string what, bool ishappening)
    {
        //not the sexist code but it works for now, 
        if (ishappening)
        {
            switch (what)
            {
                case "SlownDown" when ishappening == true:
                    duckMovement.SlowDownDuck();
                    duckFace.RandomFace(false);
                    duckSound.DuckSFX(DucksQuack.SampledClips.quackSadSFX);
                    break;
                case "SpeedUp" when ishappening == true:
                    duckMovement.SpeedUpDuck();
                    duckFace.RandomFace(true);
                    duckSound.DuckSFX(DucksQuack.SampledClips.quackHappySFX);
                    break;
                case "RockHitDuck" when ishappening == true:
                    if (duckItems.usingIntertube)
                    {
                        duckItems.CleanUpItemBoxes();
                        return;
                    }
                    duckColor.SpriteSpin();
                    duckMovement.SpeedStop();
                    duckSound.DuckSFX(DucksQuack.SampledClips.quackSadSFX);
                    duckFace.RandomFace(false);
                    duckSound.DuckSFX(DucksQuack.SampledClips.IntertubeDespawnSFX);
                    StartCoroutine(ReturnToDefault());
                    DuckTouchedWater();
                    break;
                case "Boulder" when ishappening == true:
                    duckMovement.SpeedStop();
                    duckFace.RandomFace(false);
                    duckSound.DuckSFX(DucksQuack.SampledClips.quackSadSFX);
                    GetOutOfRocket();
                    // TODO: Text Manager I hit a rock with name.
                    break;
                case "GotItem":
                    duckItems.GetItem();
                    duckSound.DuckSFX(DucksQuack.SampledClips.getItemSFX);
                    // TODO: Text Manager got item with name.
                    break;
                case "FinishLine":
                    GameManager.instance.DucksCrossedFinishLine(myName);
                    break;
            }
        }
        else
        {
            switch (what)
            {
                case "SlownDown":
                    duckMovement.ResetSpeed();
                    duckFace.DefaultFace();
                    break;
                case "SpeedUp":
                    duckMovement.ResetSpeed();
                    duckFace.DefaultFace();
                    break;
                case "Boulder":
                    duckMovement.ResetSpeed();
                    duckFace.DefaultFace();
                    break;

            }
        }
    }

    private IEnumerator ReturnToDefault()
    {
        yield return new WaitForSeconds(2);
        duckMovement.ResetSpeed();
        duckFace.DefaultFace();
    }
}
