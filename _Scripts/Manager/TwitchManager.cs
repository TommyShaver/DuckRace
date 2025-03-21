using System;
using System.IO;
using System.Net.Sockets;
using UnityEngine;


public class TwitchManager : MonoBehaviour
{
    public static TwitchManager instance;
    private TcpClient twitchClient;
    private StreamReader reader;
    private StreamWriter writer;


    private string username = "justinfan1234";
    private string passsword = "testtesttest";
    private string channelName = "plus5armor"; // <- this need to be public for user to change.
    private string trustedMod; //<- all mod name

    public bool gameStarted;
    public bool canLoadIn;

    public delegate void ChatMessageListener(string message, string parameters);
    public event ChatMessageListener ChatmessageListeners;


    //Set up unity ---------------------------------------------------------------------
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        WhichPlatform();
    }

    private void Update()
    {
        ReadChatComputer();
    }

    // Which platform are we using ----------------------------------------------------
    private void WhichPlatform()
    {
        TryToConnectTwitchComputer();
    }

    //Logic ----------------------------------------------------------------------------
    private void TryToConnectTwitchComputer()
    {
        try
        {
            twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);
            reader = new StreamReader(twitchClient.GetStream());
            writer = new StreamWriter(twitchClient.GetStream());

            writer.WriteLine("PASS " + passsword);
            writer.WriteLine("NICK " + username);
            writer.WriteLine("JOIN #" + channelName);
            writer.Flush();

            canLoadIn = true;
            Debug.Log("Connected to Twitch IRC");
        }
        catch
        {
            Debug.Log("This did not work.");
        }
    }

    private void ReadChatComputer()
    {
        if (twitchClient.Available > 0)
        {
            string message = reader.ReadLine();
            if (message.Contains("PING"))
            {
                writer.WriteLine("PONG");
                writer.Flush();
                return;
            }

            if (message.Contains("PRIVMSG") && canLoadIn)
            {
                int splitPoint = message.IndexOf("!", 1);
                string chatName = message.Substring(1, splitPoint - 1);
                splitPoint = message.IndexOf(":", 1);
                string chatMessage = message.Substring(splitPoint + 1);
                ChatmessageListeners?.Invoke(chatName, chatMessage);
                Debug.Log(chatName + " "  + chatMessage);
                if (chatMessage == "!Join" && !gameStarted)
                {
                    SpawnManager.Instance.IncomingData(chatName); //To Spawn Manager
                }
                if (chatName == trustedMod && chatMessage == "!Go")
                {
                    GameManager.instance.GameStart();
                }
                if (chatName == trustedMod && chatMessage == "!Reset")
                {
                    GameManager.instance.GameReset();
                }
                if (chatName == trustedMod && chatMessage == "!Clear")
                {
                    GameManager.instance.GameClear();
                }
                Debug.Log(canLoadIn);
            }
        }
    }


    //UI Function ------------------------------------------------------------------------
    public void ReconnectToTwitch()
    {
        WhichPlatform();
    }

    public void SwitchTwitchUserName(string s)
    {
        channelName = s;
        WhichPlatform();
        Debug.Log("Steamer is: " + s);
    }
    public void SwitchTwitchTrustedMod(string s)
    {
        trustedMod = s;
        Debug.Log("Trusted Mod is: " + s);
    }

}
