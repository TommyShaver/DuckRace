using System;
using System.IO;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using UnityEngine;


public class TwitchManager : MonoBehaviour
{
    public static TwitchManager instance;
    private TcpClient twitchClient;
    private StreamReader reader;
    private StreamWriter writer;
    private float lastPingTime;

    private string username = "justinfan1234";
    private string passsword = "testtesttest";
    private string channelName;

    public static event Action<string, string> OnChatMessage;


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
        channelName = SaveDataManager.instance.twitchStreamer;
        WhichPlatform();
    }

    private void Update()
    {

         if (twitchClient != null && twitchClient.Connected)
        {
            ReadChatComputer();

            // Send periodic PING/PONG keepalive every ~4 minutes
            if (Time.time - lastPingTime > 240f)
            {
                writer.WriteLine("PING :tmi.twitch.tv");
                writer.Flush();
                lastPingTime = Time.time;
            }
    }
    else
    {
        // Try reconnect
        TryToConnectTwitchComputer();
    }
        
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

            Debug.Log("Connected to Twitch IRC " + channelName);
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

            if (message.Contains("PRIVMSG"))
            {
                int splitPoint = message.IndexOf("!", 1);
                string chatName = message.Substring(1, splitPoint - 1);
                splitPoint = message.IndexOf(":", 1);
                string chatMessage = message.Substring(splitPoint + 1);
                string updatedMessage = CleanUpMessage(chatMessage);
                OnChatMessage?.Invoke(chatName, updatedMessage);
                //Debug.Log(chatName + " " + updatedMessage + " " + chatMessage);
                //Debug.Log($"'{chatMessage}' length:{chatMessage.Length}");
                Debug.Log($"'{updatedMessage}' length:{updatedMessage.Length}");
            }
        }
    }

    private string CleanUpMessage(string input)
    {
        string updatedString;
        updatedString = input = Regex.Replace(input, @"[\p{C}\p{Z}\u034F]+", "");
        //Debug.Log(input + ":" + updatedString + ":");
        //Debug.Log($"length:{updatedString.Length}");
        bool containsEmoji = Regex.IsMatch(input, @"[\uD800-\uDBFF][\uDC00-\uDFFF]");
        if (containsEmoji)
        {
            return input.Substring(0, input.Length - 3);
        }
        Debug.Log(updatedString + ": Bottom of stack");
        return input;
    }

    public void SwitchStreamers(string name)
    {
        channelName = name;
        WhichPlatform();
    }
}
