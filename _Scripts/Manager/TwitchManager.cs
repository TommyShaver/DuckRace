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


    private string username = "justinfan1234";
    private string passsword = "testtesttest";
    private string channelName = "plus5armor"; // <- this need to be public for user to change.
    private string trustedMod; //<- all mod name

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

            if (message.Contains("PRIVMSG"))
            {
                int splitPoint = message.IndexOf("!", 1);
                string chatName = message.Substring(1, splitPoint - 1);
                splitPoint = message.IndexOf(":", 1);
                string chatMessage = message.Substring(splitPoint + 1);
                string updatedMessage = CleanUpMessage(chatMessage);
                OnChatMessage?.Invoke(chatName, updatedMessage);
                Debug.Log(chatName + " " + updatedMessage);
            }
        }
    }

    private string CleanUpMessage(string input)
    {
        bool containsEmoji = Regex.IsMatch(input, @"[\uD800-\uDBFF][\uDC00-\uDFFF]");
        if (containsEmoji) 
        {
            return input.Substring(0, input.Length - 3);
        }
        return input;
    } 
}
