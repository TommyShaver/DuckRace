using System;
using System.Collections.Generic;
using UnityEngine;

public class TwitchEventListner : MonoBehaviour
{
    public static TwitchEventListner instance {  get; private set; }

    private bool canSpanw = true;
    private string twitchUserName = "plus5armor";
    private string trustedMod;

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
       "none", "bow", "cowboy", "cheif", "navi",
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
        //twitchUserName = SaveDataManager.instance.twitchStreamer;
        //trustedMod = SaveDataManager.instance.trustedMod;
    }



    //Helper commands -----------------------------------
    public void AddTwitchUser(string user)
    {
        twitchUserName = user;
    }
    public void AddTrustedMod(string user)
    {
        trustedMod = user;
    }

    public void GameStarted(bool isaloud)
    {
        canSpanw = isaloud;
    }






    //Inbound chat commands ------------------------------
    private void ChatListner(string user, string message)
    {
        //Are we in game?
        if (!canSpanw)
        {
            return;
        }

        ChatCommands(user, message);
    }


    private void ChatCommands(string user, string message)
    {
        //Bring text to lower for all commands some people like to do !join or !Join
        string toLowerCase = message.ToLower();
        if (user == trustedMod || user == twitchUserName)
        {
            switch (toLowerCase)
            {
                case "!reset":
                    GameManager.instance.GameReset();
                    break;
                case "!clear":
                    GameManager.instance.GameClear();
                    break;
                case "!go":
                    GameManager.instance.GameStart();
                    break;
                case "!start":
                    GameManager.instance.GameStart();
                    break;
            }

            if(message.StartsWith("!ban @", StringComparison.OrdinalIgnoreCase))
            {
                string playerBanned = message.Substring(6).ToLowerInvariant();
                if(playerBanned != twitchUserName)
                {
                    OnBanPlayer?.Invoke(playerBanned);
                }
            }
            if (message.StartsWith("!remove @", StringComparison.OrdinalIgnoreCase))
            {
                string playerBanned = message.Substring(9).ToLowerInvariant();
                if (playerBanned != twitchUserName)
                {
                    OnClearPlayer?.Invoke(playerBanned);
                }
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
                OnDuckYChange?.Invoke(user, "down");
                break;
            case "!item":
                OnDuckItemUse?.Invoke(user);
                break;
        }

        if (message.StartsWith("!color ", StringComparison.OrdinalIgnoreCase))
        {
            string color = message.Substring(7).ToLowerInvariant();
            if (validColors.Contains(color))
            {
                OnColorChangeChat?.Invoke(user, "color", color);
            }
        }

        if (message.StartsWith("!hat ", StringComparison.OrdinalIgnoreCase))
        {
            string hat = message.Substring(5).ToLowerInvariant();
            if (validHats.Contains(hat))
            {
                OnHatChangeChat?.Invoke(user, "hat", hat);
            }
        }

        //do the same with hats
    }
}
