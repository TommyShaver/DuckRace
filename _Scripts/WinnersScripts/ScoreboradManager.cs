using UnityEngine;
using TMPro;

public class ScoreboradManager : MonoBehaviour
{
    public static ScoreboradManager instance { get; private set; }
    public TextMeshProUGUI[] allNames;
    [SerializeField] int[] scores = new int[21];
    [SerializeField] string[] playerNames = new string[21];
    public TextMeshProUGUI cheatingText;
    private string whoWonLastGame;
    private bool backToBackWins;
    private string[] cheatingLineText = {": Has Host Advantage", ": Ref, do something! They won again", ": Is on fire!", ": They are owning everyone!", ": Is the star of the show!", ": Is cheating",
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

    private void OnEnable()
    {
        TwitchEventListner.OnBanPlayer += RemoveName;
        TwitchEventListner.OnClearPlayer += RemoveName;
    }

    private void OnDisable()
    {
        TwitchEventListner.OnBanPlayer -= RemoveName;
        TwitchEventListner.OnClearPlayer -= RemoveName;
    }

    private void Start()
    {
        ClearName();
    }

    //Get name on spawn ------------------------------------
    public void GetNameOnSpawn(string grabName)
    {
        for (int i = 0; i < playerNames.Length; i++)
        {
            if (playerNames[i] == string.Empty)
            {
                playerNames[i] = grabName;
                return;
            }
        }
    }

    //Give Point from game manager -------------------------
    public void GivePoint(string winner)
    {
        
        for (int i = 0; i < playerNames.Length; i++)
        {
            if (playerNames[i] == winner)
            {
                scores[i]++;
            }
        }
        if (whoWonLastGame == winner)
        {
            Debug.Log("we made it here back to back wins if statment");
            backToBackWins = true;
        }
        else
        {
            Debug.Log("we made it here back to back wins else statment");
            whoWonLastGame = winner;
            backToBackWins = false;
            cheatingText.text = string.Empty;
        }
    }

    //Draw winner names once drawn ---------------------
    public void ShowScoreOnDraw()
    {
        for (int i = 0; i < playerNames.Length; i++)
        {
            if (playerNames[i] != string.Empty)
            {
                allNames[i].text = string.Empty;
                allNames[i].text = playerNames[i] + " " + scores[i];  
            }
        }
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

    public void RemoveName(string playernameToRemove)
    {
       for (int i = 0; i < playerNames.Length; i++)
        {
            if (playerNames[i] == playernameToRemove)
            {
                allNames[i].text = string.Empty;
                playerNames[i] = string.Empty;
                scores[i] = 0;
                return;
            }
        }
    }

    public void ClearName()
    {
        for (int i = 0; i < allNames.Length; i++)
        {
            allNames[i].text = string.Empty;
            playerNames[i] = string.Empty;
            scores[i] = 0;
        }
    }
}
