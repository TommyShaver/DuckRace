using System.Collections.Generic;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    public static SaveDataManager instance;

    private HashSet<string> banPlayers = new HashSet<string>();
    private string twitchStreamer;
    private string truedtedMod;


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


    //Incoming info ----------------------------------------------
    public void BanPlayerFromJoining(string name)
    {
        banPlayers.Add(name);
    }


    public bool CheckBannedPlayers(string name) //Check to see if player is banned?
    {
        return banPlayers.Contains(name);
    }
}
