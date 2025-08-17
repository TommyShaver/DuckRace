using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class SaveDataManager : MonoBehaviour
{
    public static SaveDataManager instance;

    public HashSet<string> banPlayers = new HashSet<string>();
    public string twitchStreamer = "plus5armor";
    public string trustedMod;
    public bool rockSpawn = true;
    public bool itemsSpawn = true;
    public bool autoPlayGo = true;
    public bool firstLoad;

    

    private string savePath => Application.persistentDataPath + "/savefile.json";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            LoadDataOnLunach();
        }
        else
        {
            Destroy(this);
        }
        LoadDataOnLunach();
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

    public void SaveGameData()
    {
        SaveData data = new SaveData
        {
            streamerNameSave = twitchStreamer,
            trustedModSave = trustedMod,
            bannedPlayer = new List<string>(banPlayers),
            spawnRocks = rockSpawn,
            spawnItems = itemsSpawn,
            autoPlay = autoPlayGo
        };
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Game Saved to: " + savePath);
    }

    public void LoadDataOnLunach()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            twitchStreamer = data.streamerNameSave;
            trustedMod = data.trustedModSave;
            banPlayers = new HashSet<string>(data.bannedPlayer);
            rockSpawn = data.spawnRocks;
            itemsSpawn = data.spawnItems;
            autoPlayGo = data.autoPlay;
            Debug.Log("Game Loaded from : " + savePath);
        }
        else
        {
            Debug.LogWarning("No save file found at: " + savePath);
        }
    }
}

[System.Serializable]
public class SaveData
{
    public string streamerNameSave;
    public string trustedModSave;
    public bool spawnRocks;
    public bool spawnItems;
    public bool autoPlay;
    public List<string> bannedPlayer = new List<string>();
}
