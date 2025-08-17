using UnityEngine;
using TMPro;
using System;

public class UIButtonScript : MonoBehaviour
{

    private TextMeshProUGUI buttonText;
    private bool outBoundCommad;

    public static event Action<bool, string> OnUIButtonSwitch;
    public enum ButtonFunciton
    {
        StreamerConnect,
        ModConncect,
        AutoPlay,
        Items,
        Rocks,
        Window,
        MainMenu,
        Quit
    };

    public ButtonFunciton buttonFunciton;

    private void Awake()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        switch (buttonFunciton)
        {
            case ButtonFunciton.AutoPlay:
                buttonText.text = ChangeTextOnLoad(SaveDataManager.instance.autoPlayGo);
                break;
            case ButtonFunciton.Items:
                buttonText.text = ChangeTextOnLoad(SaveDataManager.instance.itemsSpawn);
                break;
            case ButtonFunciton.Rocks:
                buttonText.text = ChangeTextOnLoad(SaveDataManager.instance.rockSpawn);
                break;
            case ButtonFunciton.Window:

                break;
        }
    }

    public void ButtonLogic()
    {
        switch (buttonFunciton)
        {
            case ButtonFunciton.StreamerConnect:
                if (TryGetComponent(out ConnectInputfieldScript connectInputfieldStreamer))
                {
                    TwitchEventListner.instance.AddTwitchStreamer(connectInputfieldStreamer.GetText());
                    SaveDataManager.instance.twitchStreamer = connectInputfieldStreamer.GetText();
                    TwitchManager.instance.SwitchStreamers(connectInputfieldStreamer.GetText());
                }
                break;
            case ButtonFunciton.ModConncect:
                if (TryGetComponent(out ConnectInputfieldScript connectInputfieldMod))
                {
                    TwitchEventListner.instance.AddTrustedMod(connectInputfieldMod.GetText());
                    SaveDataManager.instance.trustedMod = connectInputfieldMod.GetText();
                }
                break;
            case ButtonFunciton.AutoPlay:
                buttonText.text = ChangeTextOnOff(buttonText);
                OnUIButtonSwitch?.Invoke(outBoundCommad, "auto");
                SaveDataManager.instance.autoPlayGo = outBoundCommad;
                break;
            case ButtonFunciton.Items:
                buttonText.text = ChangeTextOnOff(buttonText);
                OnUIButtonSwitch?.Invoke(outBoundCommad, "item");
                SaveDataManager.instance.itemsSpawn = outBoundCommad;
                break;
            case ButtonFunciton.Rocks:
                buttonText.text = ChangeTextOnOff(buttonText);
                OnUIButtonSwitch?.Invoke(outBoundCommad, "rock");
                SaveDataManager.instance.rockSpawn = outBoundCommad;
                break;
            case ButtonFunciton.Window:
                buttonText.text = SettingsScript.instance.SwitchScreenSize();
                break;
            case ButtonFunciton.MainMenu:
                break;
            case ButtonFunciton.Quit:
                SettingsScript.instance.CloseApp();
                break;
        }
        SaveDataManager.instance.SaveGameData();
    }

    private string ChangeTextOnOff(TextMeshProUGUI text)
    {
        string updatedText;
        if (text.text == "Off")
        {
            updatedText = "On";
            outBoundCommad = true;
        }
        else
        {
            updatedText = "Off";
            outBoundCommad = false;
        }
        return updatedText;
    }
    private string ChangeTextOnLoad(bool whichState)
    {
        string updateText = "";
        if (whichState)
        {
            updateText = "On";
        }
        else
        {
            updateText = "Off";
        }
        return updateText;
    }
}
