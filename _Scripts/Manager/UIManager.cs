using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;
using System;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject BlackOverlay;
    public TMP_InputField switchUserName;
    public TMP_InputField switchTrustedMod;
    public TextMeshProUGUI placeHolderTextMod; // Show current mod
    public TextMeshProUGUI showTrusedMod;
    public TextMeshProUGUI placeHolderText; // Show current user name.
    public TextMeshProUGUI showConnectedUserName; //Show again just in case.
    public RectTransform UIMenuRectTracs;
    public RectTransform UIGoButtonTrans;
    public RectTransform UIResetEogButton;
    public RectTransform UIClearEogButton;
    public RectTransform joinTextEffectTransform;
    public TextMeshProUGUI joinText;
    public TextMeshProUGUI winnerNameText;
    public RectTransform[] endBorader;
    public TextMeshProUGUI[] namePlateText;
    public RectTransform[] namePlateObject;
    public TextMeshProUGUI hotStreakText;
    public GameObject resetButtonMenu;
    public Slider slider_SFX;
    private bool isMenuOpen;
    private bool canMove = true;
    private string firstLoad;
    private int place;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        switchUserName.onEndEdit.AddListener(U_OnEndEdit);
        showConnectedUserName.text = string.Empty;
        LoadData();
        UIGoButtonTrans.DOAnchorPosY(-50, 0, false).SetEase(Ease.OutBack);
        UIResetEogButton.DOAnchorPosY(-1000, 0, false);
        UIClearEogButton.DOAnchorPosY(-1000, 0, false);
        joinText.text = "!Join to join the game";
        winnerNameText.text = string.Empty;
        if(firstLoad != "IsTrue")
        {
            U_OpenMenu();
            firstLoad = "IsTrue";
            PlayerPrefs.SetString("FirstLoad", "IsTrue");
        }
        for(int i = 0; i <namePlateObject.Length; i++)
        {
            namePlateText[i].text = string.Empty;
            namePlateObject[i].DOAnchorPosX(-200f, 0);
        }
        resetButtonMenu.SetActive(false);
        joinText.fontSize = 75;
        slider_SFX.value = 1f;
    }

    //Load local user names -------------------------------------------------------------------------------
    private void LoadData()
    {
        TwitchManager.instance.SwitchTwitchUserName(PlayerPrefs.GetString("Streamer"));
        placeHolderText.text = PlayerPrefs.GetString("Streamer") + " connected";
        showConnectedUserName.text = "Connected to " + PlayerPrefs.GetString("Streamer");

        TwitchManager.instance.SwitchTwitchTrustedMod(PlayerPrefs.GetString("TrustedMod"));
        placeHolderTextMod.text = PlayerPrefs.GetString("TrustedMod") + " connected";
        showTrusedMod.text = "Connected to " + PlayerPrefs.GetString("TrustedMod");

        firstLoad = PlayerPrefs.GetString("FirstLoad");
    }

    //Main Menu Logic --------------------------------------------------------------------------------------
    public void U_OpenMenu()
    {
        MenuMovement();
    }

    private void MenuMovement()
    {
        if (canMove)
        {
            canMove = false;
            StartCoroutine(WaitForAnimation());
            if (!isMenuOpen)
            {
                UIMenuRectTracs.DOAnchorPosX(-550, .5f, false).SetEase(Ease.OutBack).SetUpdate(true);
                isMenuOpen = true;
                BlackOverlay.SetActive(true);
                //Sound Manager;
            }
            else
            {
                //GameManager.instance.TimeMagament(1);
                UIMenuRectTracs.DOAnchorPosX(500, .5f, false).SetEase(Ease.InOutBack).SetUpdate(true);
                isMenuOpen = false;
                BlackOverlay.SetActive(false);
                //Sound Manager;
            }
        }
    }

    public void U_OnEndEdit(string inputText) //Change username
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            PlayerPrefs.SetString("Streamer", inputText);    
            TwitchManager.instance.SwitchTwitchUserName(inputText.ToLower());
            placeHolderText.text = inputText + " connected";
            showConnectedUserName.text = "Connected to "  + inputText;
        }
    }

    public void U_OnEditTrustedMod(string inputText) // Chane Trusted mod
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            PlayerPrefs.SetString("TrustedMod", inputText);
            TwitchManager.instance.SwitchTwitchTrustedMod(inputText.ToLower());   
            placeHolderTextMod.text = inputText + " connected";
            showTrusedMod.text = "Connected to " + inputText;
            if(inputText == "Duck!")
            {
                SoundManager.instance.SerectSong();
                showTrusedMod.text = "-_- You found me...";
            }
        }
    }

    public void GameOverButtons(bool visable)
    {
        if(visable)
        {
            UIClearEogButton.DOAnchorPosY(-200, .5f).SetEase(Ease.OutQuad);
            UIResetEogButton.DOAnchorPosY(-200, .5f).SetEase(Ease.OutQuad);
        }
        else
        {
            UIResetEogButton.DOAnchorPosY(-1000, .5f, false).SetEase(Ease.OutQuad);
            UIClearEogButton.DOAnchorPosY(-1000, .5f, false).SetEase(Ease.OutQuad);
        }
    }

    //Global UI Management ----------------------------------------------------------------------------------------------------
    public void ShowHideStartButton(bool active)
    {
        if (active) 
        {
            joinText.text = string.Empty;
            UIGoButtonTrans.DOAnchorPosY(50, .5f, false).SetEase(Ease.OutBack);
        }
        else
        {
            UIGoButtonTrans.DOAnchorPosY(-50, .5f, false).SetEase(Ease.OutBack);
        }
    }

    public void AnimateCountDownText(string counDown, float timeGameManager)
    {
        joinText.fontSize = 400;
        joinText.text = counDown;
        joinTextEffectTransform.DOShakeAnchorPos(timeGameManager, 50);
        joinText.DOFade(1, 0).SetEase(Ease.OutBounce);
        StartCoroutine(CountDown(timeGameManager));
    }

    //End Of Game logic -------------------------------------------------------------------------------------------------------
    public void EndOfGameBorder()
    {
        for(int i = 0; i < endBorader.Length; i++)
        {
            if(i == 0)
            {
                endBorader[0].DOAnchorPosY(500, 2).SetDelay(1).SetEase(Ease.OutBack);
            }
            else
            {
                endBorader[1].DOAnchorPosY(-500, 2).SetDelay(1).SetEase(Ease.OutBack);
            }
        }
    }

    public void NamePlateLogic(string name)
    {
        if(place < 3)
        {
            namePlateObject[place].DOAnchorPosX(150, .5f).SetEase(Ease.InOutBack);
            namePlateText[place].text = name;
        }
        place++;
        //Play Sound
    }
    public void ResetEndOfGameUI()
    {
        for (int i = 0; i < namePlateObject.Length; i++)
        {
            namePlateText[i].text = string.Empty;
            namePlateObject[i].DOAnchorPosX(-200f, .5f).SetEase(Ease.InOutBack);
        }
        for (int i = 0; i < endBorader.Length; i++)
        {
            if (i == 0)
            {
                endBorader[0].DOAnchorPosY(600, 1).SetEase(Ease.OutBack);
            }
            else
            {
                endBorader[1].DOAnchorPosY(-600, 1).SetEase(Ease.OutBack);
            }
        }
        place = 0;
    }

    private IEnumerator WaitForAnimation()
    {
        yield return new WaitForSecondsRealtime(.5f);
        canMove = true;
    }

    public IEnumerator CountDown(float timeTill)
    {
        yield return new WaitForSeconds(timeTill);
        joinText.DOFade(0, .2f);
    }

    public void Slider_SFX()
    {
        float volume = slider_SFX.value;
        SoundManager.instance.AudioMixerSetting(volume);
        Debug.Log(volume);
    }
}
