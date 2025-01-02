using System.Collections;
using System.IO;
using System.Net.Sockets;
using UnityEngine;


public class TwitchManager : MonoBehaviour
{
    private TcpClient twitchClient;
    private StreamReader reader;
    private StreamWriter writer;

    private string username = "justinfan1234";
    private string passsword = "testtesttest";
    private string channelName = "Dark_Zelda92"; // <- this need to be public for user to change.

    public delegate void ChatMessageListener(string message, string parameters);
    public event ChatMessageListener ChatmessageListeners;


    //Set up unity ---------------------------------------------------------------------
    private void Start()
    {
        TryToConnectTwitch();
    }

    private void Update()
    {
        ReadChat();
    }

    //Logic ----------------------------------------------------------------------------
    private void TryToConnectTwitch()
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

    private void ReadChat()
    {
        if(twitchClient.Available > 0)
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
                ChatmessageListeners?.Invoke(chatName, chatMessage);
                Debug.Log(chatName + " "  + chatMessage);
                if(chatMessage == "!Join")
                {
                    SpawnManager.Instance.IncomingData(chatName); //To Spawn Manager
                }
            }
        }
    }

    //UI Function ------------------------------------------------------------------------
    public void ReconnectToTwitch()
    {
        TryToConnectTwitch();
    }
    
}
