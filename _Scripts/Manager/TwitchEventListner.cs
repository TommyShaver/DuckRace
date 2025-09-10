using System;
using System.Collections.Generic;
using UnityEngine;

public class TwitchEventListner : MonoBehaviour
{
    public static TwitchEventListner instance {  get; private set; }

    public bool canRemove;

    private string twitchUserName;
    private string trustedMod;
    private int botNumber;

    public static event Action<string, string, string> OnColorChangeChat;
    public static event Action<string, string, string> OnHatChangeChat;
    public static event Action<string, string> OnDuckYChange;
    public static event Action<string> OnDuckQuack;
    public static event Action<string> OnDuckTaunt;
    public static event Action<string> OnDuckItemUse;
    public static event Action<string> OnBanPlayer;
    public static event Action<string> OnClearPlayer;

    HashSet<string> validColors = new HashSet<string>()
    {
         "red", "blue", "green", "purple", "yellow", "orange",
         "pink", "white", "black", "brown", "random", "rainbow"
    };


    HashSet<string> validHats = new HashSet<string>()
    {
       "none", "bow", "cowboy", "chief", "navi",
       "santa", "party", "witch", "warlock", "straw"
    };
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        TwitchManager.OnChatMessage += ChatListner;
    }
    private void OnDisable()
    {
        TwitchManager.OnChatMessage -= ChatListner;
    }

    private void Start()
    {
        canRemove = true;
        twitchUserName = SaveDataManager.instance.twitchStreamer;
        trustedMod = SaveDataManager.instance.trustedMod;
        Debug.Log(twitchUserName + ":" + trustedMod);
    }



    //Helper commands -----------------------------------
    public void AddTwitchStreamer(string user)
    {
        twitchUserName = user;
        UnityEngine.Debug.Log(user + ": TwitchEventListner " + twitchUserName);
    }
    public void AddTrustedMod(string user)
    {
        trustedMod = user;
        UnityEngine.Debug.Log(user + ": TwitchEventListner " + trustedMod);
    }


    //Inbound chat commands ------------------------------
    public void GameManagerCanRemove(bool remove)
    {
        canRemove = remove;
    }
    private void ChatListner(string user, string message)
    {
        ChatCommands(user, message);
    }


    private void ChatCommands(string user, string message)
    {
        //Debug.Log(user + ":" + message);
        //Bring text to lower for all commands some people like to do !join or !Join
        string toLowerCase = message.ToLower();
        if (user == twitchUserName || user == trustedMod)
        {
            //UnityEngine.Debug.Log("User from Twitch Manager: " + user + " User from Twitch Listener: " + twitchUserName);
            switch (toLowerCase)
            {
                case "!reset":
                    GameManager.instance.GameReset();
                    SoundManager.instance.StopAllPlayers();
                    MusicManager.instance.StopAllPlayers();
                    break;
                case "!clear":
                    GameManager.instance.GameClear();
                    SoundManager.instance.StopAllPlayers();
                    MusicManager.instance.StopAllPlayers();
                    break;
                case "!go":
                    GameManager.instance.GameStart();
                    break;
                case "!start":
                    GameManager.instance.GameStart();
                    break;
                case "!menu":
                    ShowMenuScripts.instance.InBoundCommandMenuAnimation();
                    break;
                case "!stop":
                    GameManager.instance.StopAutoPlay();
                    break;
                case "!addplayer":
                    botNumber++;
                    SpawnManager.Instance.IncomingData($"BillBot:{botNumber}");
                    break;
            }

            if (message.StartsWith("!ban@", StringComparison.OrdinalIgnoreCase))
            {
                string playerBanned = message.Substring(5).ToLowerInvariant();
                PlayerWarningTextScript.instance.SwitchPlayerWarningText($"{playerBanned} has been banned");
                if (playerBanned != twitchUserName)
                {
                    OnBanPlayer?.Invoke(playerBanned);
                }
            }

            if (message.StartsWith("!unban@", StringComparison.OrdinalIgnoreCase))
            {
                string playerBanned = message.Substring(7).ToLowerInvariant();
                PlayerWarningTextScript.instance.SwitchPlayerWarningText($"{playerBanned} has been un-banned");
                if (playerBanned != twitchUserName)
                {
                    SaveDataManager.instance.UnBanPlayerFromJoining(playerBanned);
                }
            }

            if (message.StartsWith("!remove@", StringComparison.OrdinalIgnoreCase) && canRemove)
            {
                string playerClear = message.Substring(8).ToLowerInvariant().TrimEnd();
                PlayerWarningTextScript.instance.SwitchPlayerWarningText($"{playerClear} has been removed");
                OnClearPlayer?.Invoke(playerClear);
                SoundManager.instance.PlaySFXFromSoundEffect(SoundManager.SFX_Clip.Clear_Duck_SFX);
                Debug.Log($"we made it here {playerClear}: they should be gone");
            }
        }

        //These commands are for everyone else.
        switch (toLowerCase)
        {
            //Basic Commands
            case "!join":
                SpawnManager.Instance.IncomingData(user);
                break;
            case "!taunt":
                OnDuckTaunt?.Invoke(user);
                break;
            case "!quack":
                OnDuckQuack?.Invoke(user);
                break;
            case "!up":
                OnDuckYChange?.Invoke(user, "up");
                break;
            case "!down":
                Debug.Log(user + ":" + message + "<color=Yellow>This is inside the switch</color>");
                OnDuckYChange?.Invoke(user, "down");
                break;
            case "!item":
                //Debug.Log("Twitch event item:" + toLowerCase);
                OnDuckItemUse?.Invoke(user);
                break;
        }

        if (message.StartsWith("!color", StringComparison.OrdinalIgnoreCase))
        {
            string color = message.Substring(6).ToLowerInvariant();
            if (validColors.Contains(color))
            {
                OnColorChangeChat?.Invoke(user, "color", color);
            }
        }

        if (message.StartsWith("!hat", StringComparison.OrdinalIgnoreCase))
        {
            string hat = message.Substring(4).ToLowerInvariant();
            if (validHats.Contains(hat))
            {
                OnHatChangeChat?.Invoke(user, "hat", hat);
            }
        }
        //Debug.Log("Bottom of the block" + user + message);
    }
}
