using UnityEngine;
using TMPro;
public class UsernameScripts : MonoBehaviour
{
    public bool isStreamer;

    [SerializeField] TextMeshProUGUI placeHolderText;

    private void Start()
    {
        if (isStreamer)
        {
            placeHolderText.text = SaveDataManager.instance.twitchStreamer;
        }
        else
        {
            placeHolderText.text = SaveDataManager.instance.trustedMod;
        }
    }
    public void GetName(string input)
    {
        if (isStreamer)
        {
            TwitchEventListner.instance.AddTwitchStreamer(input.ToLower());
            SaveDataManager.instance.twitchStreamer = input.ToLower();
            placeHolderText.text = input;
        }
        else
        {
            TwitchEventListner.instance.AddTrustedMod(input.ToLower());
            SaveDataManager.instance.trustedMod = input.ToLower();
            placeHolderText.text = input;
        }
    }
}
