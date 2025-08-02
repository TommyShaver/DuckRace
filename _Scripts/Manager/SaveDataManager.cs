using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class SaveDataManager : MonoBehaviour
{
    public static SaveDataManager instance;

    public HashSet<string> banPlayers = new HashSet<string>();
    public string twitchStreamer;
    public string trustedMod;

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

    public void SaveDataOnQuit()
    {
        SaveData data = new SaveData
        {
            streamerNameSave = twitchStreamer,
            trustedModSave = trustedMod,
            bannedPlayer = new List<string>(banPlayers)
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
    public List<string> bannedPlayer = new List<string>();
}
