using UnityEngine;
using DG.Tweening;
using System.Collections;
public class IntroAnimation : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject logoPlus5Armor;
    public GameObject maskXAnim;
    public SpriteRenderer logoDarkZelda;
    public SpriteRenderer fadeBlack;
    public GameObject duckFlyBy;
    public GameObject title;
    public ParticleSystem feathersFly;
    public GameObject skipText; 

    [Header("Audio Sources")]
    public AudioSource SFX_Player1;
    public AudioSource SFX_Atmo;
    public AudioSource MusicPlayer;

    [Header("Audio Clip")]
    public AudioClip plus5ArmorSFXClip;
    public AudioClip darkZSFXClip;
    public AudioClip duckFlyBySFXClip;
    public AudioClip atmoWaterSFXClip;
    public AudioClip introMusicClip;
    public AudioClip flyByLogoClipL;
    public AudioClip flyByLogoClipR;

    public float delayAudio;

    private Tween logoPlus5AnimTween;
    private Tween logoScaleTween;
    private Tween darkZAnimTween;
    private Tween stopMaskTween;
    private Tween duckFlyByAnimTween;
    private Tween stopFadeOutTween;
    private Tween audioFadeTween;

    private void Start()
    {
        AnimationOnLoad();
        if (SaveDataManager.instance.firstLoad == false)
        {
            skipText.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && SaveDataManager.instance.firstLoad == false)
        {
            StopAllCoroutines();
            StartCoroutine(SkipIntro());
        }
    }

    private void AnimationOnLoad()
    {
        StartCoroutine(AnimationIntro());
    }


    private void AudioCall(AudioSource player, AudioClip clip, bool fade, float fadeTime, float setDelay, float volume)
    {
        player.clip = clip;
        if (fade)
        {
            player.volume = 0;
            audioFadeTween = player.DOFade(volume, fadeTime);
        }
        player.PlayDelayed(setDelay);
    }

    private IEnumerator AnimationIntro()
    {
        yield return new WaitForSeconds(1);
        logoPlus5AnimTween = logoPlus5Armor.GetComponent<SpriteRenderer>().DOFade(1f, 1f);
        logoScaleTween = logoPlus5Armor.transform.DOScale(1.5f, 3f).SetEase(Ease.OutElastic);
        AudioCall(SFX_Atmo, flyByLogoClipL, false, 0, .3f, 1);
        stopMaskTween = maskXAnim.transform.DOLocalMoveX(7, 1f).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            AudioCall(SFX_Atmo, flyByLogoClipR, false, 0, .3f, 1);
            maskXAnim.transform.DOLocalMoveX(-7, 1f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                logoPlus5Armor.GetComponent<SpriteRenderer>().DOFade(0f, 0.5f);
            });
        });
        AudioCall(SFX_Player1, plus5ArmorSFXClip,false,0,0,1);



        yield return new WaitForSeconds(3);
        darkZAnimTween = logoDarkZelda.DOFade(1, 1).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            logoDarkZelda.DOFade(0, 3).SetEase(Ease.InOutSine);
        });
        AudioCall(SFX_Player1, darkZSFXClip,false,0,0,1);



        yield return new WaitForSeconds(5);
        stopFadeOutTween = fadeBlack.DOFade(0, 3).SetEase(Ease.InOutSine);
        AudioCall(SFX_Atmo, atmoWaterSFXClip,true,4,0,.5f); 
        AudioCall(SFX_Player1, duckFlyBySFXClip,false,0, 1.45f, 1.5f);
        AudioCall(MusicPlayer, introMusicClip,false,0,delayAudio,1);

        yield return new WaitForSeconds(3);
        duckFlyByAnimTween = duckFlyBy.transform.DOLocalMoveX(10, 1f).OnComplete(() =>
        {
            title.SetActive(true);
            feathersFly.Play();
        });



        yield return new WaitForSeconds(8);
        //Tell scene loader to load game scenes.
        title.SetActive(false);
        skipText.SetActive(false);
        fadeBlack.color = Color.black;
        SFX_Atmo.DOFade(0, .5f);
        fadeBlack.DOFade(1, 1).SetEase(Ease.InOutSine).OnComplete(() =>
        {
           SceneLoader.instance.AdvanceToNextScene(SceneLoader.LoadedScene.LoadingScene); 
        });
    }

    private IEnumerator SkipIntro()
    {
        logoDarkZelda.DOFade(0, 3).SetEase(Ease.InOutSine);
        fadeBlack.DOFade(0, 1).SetEase(Ease.InOutSine);
        logoPlus5Armor.GetComponent<SpriteRenderer>().DOFade(0f, 0.5f);
        stopMaskTween.Kill();
        darkZAnimTween.Kill();
        logoPlus5AnimTween.Kill();
        logoScaleTween.Kill();
        duckFlyByAnimTween.Kill();
        stopFadeOutTween.Kill();
        maskXAnim.SetActive(false);
        SFX_Player1.DOFade(0, .5f);
        SFX_Atmo.DOFade(0, .5f);
        MusicPlayer.DOFade(0, 0.5f);
        title.SetActive(false);
        skipText.SetActive(false);
        audioFadeTween.Kill();
        yield return new WaitForSeconds(3);

        SceneLoader.instance.AdvanceToNextScene(SceneLoader.LoadedScene.LoadingScene);
    }
}
