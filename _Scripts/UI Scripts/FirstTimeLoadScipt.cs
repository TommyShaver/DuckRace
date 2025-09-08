using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;

public class FirstTimeLoadScipt : MonoBehaviour
{
    public Image fadeToBlack;
    public TextMeshProUGUI duckTutoiralText;
    public TextMeshProUGUI commandsText;
    public GameObject chooseModOrStream;
    public TextMeshProUGUI pickstreamModText;
    public TextMeshProUGUI youcanSkipIfYouWantText;
    public Button selectStreamOrMod;
    public TMP_InputField inputfeild;
    public GameObject duck;
    public AudioSource advanceAudioSFX;
    public AudioSource duckTalkingAudioSource;
    public AudioSource musicBedPlayer1;
    public AudioSource musicBedPlayer2;
    public AudioClip UI_SFX;
    public AudioClip gameBegin_SFX;
    public AudioClip duckSpeaking_SFX;
    public AudioClip duckFinishSpeaking_SFX;
    public AudioClip introMusicTutoiral;
    public AudioClip mainMusicLoopTutoiral;

    private string[] duckTutoiralCommands = {//Tutoiral text 1 ................................................................
                                            "Welcome to Quack Dash \nFeathers Of Fury!!!",

                                            //Tutoiral text 2 ..................................................................
                                            "Okay, let's get started. \nPlease add your Twitch \nuser name.",

                                            //Tutoiral text 3 .................................................................
                                            "Nice! Let's also add \nsomeone you trust, like a \nmod or someone special in \nyour channel.\n\nYou can also skip this for \nright now by pressing space.",

                                            //Tutoiral text 4 .................................................................
                                            @"Awesome, let's go over \nsome commands.\nThese commands are for all \nplayers.\n\nPress the space bar when \nyou are done.",

                                            //Tutoiral text 5 .................................................................
                                            @"And these commands are \nfor in-game.\n\nPress the space bar when \nyou are done.",

                                            //Tutoiral text 6 .................................................................
                                            @"These are super important for the streamer and the trusted mod.\n\nPress the space bar when you are done.",
                                            
                                            //Tutoiral text 7 .................................................................
                                            @"Okay, now you've got all that, let's play!\n\nOk let's go! The rest of the game will be played with twitch chat!"
                                            };
    private string[] commandAndRules = {//Commands 1 ............................................................................
                                        "<u>All player Commands</u>\n<b><color=orange>!join</color></b> - lets you join the game.\n<b><color=orange>!color</color></b> <i>(type your choice) </i>- red, blue, green, purple, \nyellow, orange, pink, white, black, brown, random, \nrainbow.\nHere is an example -> <u><color=yellow>!color brown</color></u>\n<b><color=orange>!hat</color></b>  <i>(type your choice) </i>- none, bow, cowboy, chief,\nnavi, santa, party, witch, warlock, straw. \nHere is an example -> <u><color=yellow>!hat cowboy</color></u>\n",
                                        
                                        //Commands 2 ..............................................................................
                                        "<u>In-game commands are available to all players</u>\n<b><color=orange>!quack</color></b> - lets you quack your duck\n<b><color=orange>!up</color></b> - lets you move your player up\n<b><color=orange>!down</color></b> - lets you move your player down\n<b><color=orange>!item</color></b> - When you get an item, \nthis might help you win!\n<b><color=orange>!taunt</color></b> - Lets you taunt other players,\n but it will <color=red><i>cost you speed</i></color>.",
                                        
                                        //Commands 3 ................................................................................
                                        "This one is <color=red><b>Important</b></color>; this is for mods and streamers.\n<b><color=orange>!go !start</b></color> - Will start the game.\n<b><color=orange>!reset</b></color> - Will reset the game but not clear the\n current players\n<b><color=orange>!clear</b></color> - Will let you clear the board for new players.\n<b><color=orange>!menu</b></color> - Will bring up the menu to change \nsettings and screens.\n<b><color=orange>!remove</b></color>  @<i>(twitch name)</i> - Will let you remove\none player from the game.\n<b><color=orange>!ban</b></color> @<i>(twitch name) </i>- Will make it so the Twitch\n account will get removed and can't come back. \n<b><color=orange>!unban</b></color> @<i>(twitch name)</i> - Will make it so the Twitch\n account can join the game."};


    private float textScrollSpeed = 0.03f;
    private bool canAdvance;
    private Vector3 duckEndPos = new(-700, -500, 0);
    private Vector3 duckStartPos = new(-1000, -900, 0);
    private int progressionTracker;
    private bool streamChoosen;
    private bool buttonPressedOnce;
    [SerializeField] float setAudioDelay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canAdvance = false;
        duckTutoiralText.text = string.Empty;
        commandsText.text = string.Empty;
        youcanSkipIfYouWantText.text = string.Empty;
        chooseModOrStream.SetActive(false);
        fadeToBlack.DOFade(0, .5f).OnComplete(() =>
        {
            AudioCallFunction();
            duck.transform.DOLocalMove(duckEndPos, 1f).OnComplete(() =>
            {
                TurtoiralProgession(1);
                SceneLoader.instance.UnloadLastScene(SceneLoader.LoadedScene.OpeningScene);
            });
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canAdvance && streamChoosen)
            {
                TurtoiralProgession(progressionTracker);
                advanceAudioSFX.clip = UI_SFX;
                advanceAudioSFX.volume = 1f;
                advanceAudioSFX.pitch = 1.5f;
                advanceAudioSFX.Play();
            }
        }
    }

    private void AudioCallFunction()
    {
        musicBedPlayer1.clip = introMusicTutoiral;
        musicBedPlayer2.clip = mainMusicLoopTutoiral;
        musicBedPlayer1.Play();
        musicBedPlayer2.PlayDelayed(setAudioDelay);
    }

    private void FadeOutAudio()
    {
        musicBedPlayer1.DOFade(0, 1f);
        musicBedPlayer2.DOFade(0, 1f);   
    }

    private void TurtoiralProgession(int updateProgrssion)
    {
        canAdvance = false;
        duckTutoiralText.text = string.Empty;
        commandsText.text = string.Empty;
        buttonPressedOnce = false;

        switch (updateProgrssion)
        {
            case 1:
                StartCoroutine(TypeLine(duckTutoiralCommands[0], true, 2, 2));
                break;
            case 2:
                chooseModOrStream.SetActive(true);
                StartCoroutine(TypeLine(duckTutoiralCommands[1], false, 0, 3));
                pickstreamModText.text = "Add your Twitch user name:";
                youcanSkipIfYouWantText.text = string.Empty;
                break;
            case 3:
                inputfeild.text = string.Empty;
                chooseModOrStream.SetActive(true);
                StartCoroutine(TypeLine(duckTutoiralCommands[2], false, 0, 4));
                pickstreamModText.text = "Add a trusted user name:";
                youcanSkipIfYouWantText.text = "You can skip this part by pressing the space bar.";
                break;
            case 4:
                chooseModOrStream.SetActive(false);
                StartCoroutine(TypeLine(duckTutoiralCommands[3], false, 0, 5));
                commandsText.text = commandAndRules[0];
                break;
            case 5:
                StartCoroutine(TypeLine(duckTutoiralCommands[4], false, 0, 6));
                commandsText.text = commandAndRules[1];
                break;
            case 6:
                StartCoroutine(TypeLine(duckTutoiralCommands[5], false, 0, 7));
                commandsText.text = commandAndRules[2];
                break;
            case 7:
                StartCoroutine(TypeLine(duckTutoiralCommands[6], true, 2, 8));
                break;
            case 8:
                FadeOutAudio();
                duckTutoiralText.text = string.Empty;
                SaveDataManager.instance.firstLoad = false;
                SaveDataManager.instance.SaveGameData();
                advanceAudioSFX.clip = gameBegin_SFX;
                advanceAudioSFX.pitch = 1;
                advanceAudioSFX.Play();
                duck.transform.DOLocalMove(duckStartPos, 1f).OnComplete(() =>
                {
                    fadeToBlack.DOFade(1, .5f).OnComplete(() =>
                    {
                        SceneLoader.instance.AdvanceToNextScene(SceneLoader.LoadedScene.LoadingScene);
                    });
                });
                break;
        }
    }



    //Type line script .............................................................................................
    private IEnumerator TypeLine(string linesOfSpeech, bool autoProgess, int waitTimer, int whichProgression)
    {
        duckTalkingAudioSource.volume = 1f;
        duckTalkingAudioSource.pitch = 1;
        duckTalkingAudioSource.panStereo = -.5f;
        duckTalkingAudioSource.clip = duckSpeaking_SFX;
        var sb = new System.Text.StringBuilder();
        for (int i = 0; i < linesOfSpeech.Length; i++)
        {
            char c = linesOfSpeech[i];

            sb.Append(c);
            duckTutoiralText.text = sb.ToString();

            //var clip = (i < linesOfSpeech.Length - 1) ? duckSpeaking_SFX : duckFinishSpeaking_SFX; // Great way to do shorthand if statments.
            if (i % 2 == 0 || c == '.' || c == '!')
            {
                float pitch = (c == '.' || c == '!') ? 1.3f : 1f;
                duckTalkingAudioSource.pitch = pitch;
                duckTalkingAudioSource.Play();   
            }
            
            yield return new WaitForSeconds(textScrollSpeed); 
        } 
        duckTalkingAudioSource.clip = duckFinishSpeaking_SFX;
        duckTalkingAudioSource.Play(); 

        yield return new WaitForSeconds(waitTimer);  
        if (autoProgess)
        {
            TurtoiralProgession(whichProgression);
        }
        else
        {
            canAdvance = true;
            progressionTracker = whichProgression;
        }
    }


    //UI messages ...................................................................................................
    public void ButtonPressed()
    {
        if (buttonPressedOnce)
        {
            return;
        }

        if (!streamChoosen)
        {
            SaveDataManager.instance.twitchStreamer = inputfeild.text;
            TwitchManager.instance.SwitchStreamers(inputfeild.text);
            streamChoosen = true;
            buttonPressedOnce = true;
            TurtoiralProgession(3);
        }
        else
        {
            SaveDataManager.instance.trustedMod = inputfeild.text;
            buttonPressedOnce = true;
            TurtoiralProgession(4);
            chooseModOrStream.SetActive(false);
        }
        SaveDataManager.instance.SaveGameData();
    }
}
