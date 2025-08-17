using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ScoreboradManager : MonoBehaviour
{
    public static ScoreboradManager instance { get; private set; }
    public TextMeshProUGUI[] allNames;
    public TextMeshProUGUI cheatingText;

    private Dictionary<string, int> playerScore = new Dictionary<string, int>();
    private string whoWonLastGame;
    private int currentPostion;
    private bool backToBackWins;
    private string[] cheatingLineText = {": Has Host Advantage", ": Reff do something! They won again", ": Is on fire!", ": They are owning everyone!", ": Is the star of the show!", ": Is cheating",
    ": Is paying the dev to win." };

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

    private void Start()
    {
        ClearName();
    }

    //Get name on spawn ------------------------------------
    public void GetNameOnSpawn(string grabName)
    {
        playerScore.Add(grabName, 0);
    }

    //Give Point from game manager -------------------------
    public void GivePoint(string winner)
    {
        if (playerScore.ContainsKey(winner))
        {
            playerScore[winner] += 1;
        }
        if (whoWonLastGame == winner)
        {
            backToBackWins = true;
        }
        else
        {
            whoWonLastGame = winner;
            backToBackWins = false;
            cheatingText.text = string.Empty;
        }
    }

    //Draw winner names once drawn ---------------------
    public void ShowScoreOnDraw()
    {
        string display = string.Empty;
        foreach (var entry in playerScore)
        {
            display += $"{entry.Key}: {entry.Value}";
            allNames[currentPostion].text = display;
            currentPostion++;
        }
        currentPostion = 0;
        if (backToBackWins)
        {
            ShowCheatingName();
        }
    }

    private void ShowCheatingName()
    {
        if (whoWonLastGame == SaveDataManager.instance.twitchStreamer)
        {
            cheatingText.text = whoWonLastGame + cheatingLineText[UnityEngine.Random.Range(0, cheatingLineText.Length)];
        }
        else
        {
            cheatingText.text = whoWonLastGame + cheatingLineText[UnityEngine.Random.Range(1, cheatingLineText.Length)];
        }
        
    }

    public void ClearName()
    {
        currentPostion = 0;
        for (int i = 0; i < allNames.Length; i++)
        {
            allNames[i].text = string.Empty;
        }
        playerScore.Clear();
    }
}
