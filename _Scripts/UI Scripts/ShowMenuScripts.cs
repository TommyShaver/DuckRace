using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ShowMenuScripts : MonoBehaviour
{
    public static ShowMenuScripts instance { get; private set; }
    public GameObject duckGameObject;
    public GameObject menuGameObject;
    public GameObject menuTextObject;
    public GameObject barObject;

    private bool menuShown;
    private Image backgroundImage;

    private Vector3 duckStartPos = new(-2000, -900, 0);
    private Vector3 duckEndPos = new(-750, -300, 0);

    private float settingsMoveXStartPos = 1600;
    private float settingMoveXEndPos = 0;
    private AudioSource audioSource;

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
        backgroundImage = GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        CleanUp();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            InBoundCommandMenuAnimation();
        }
    }

    public void InBoundCommandMenuAnimation()
    {
        if (menuShown)
        {
            SlideOutMenu();
        }
        else
        {
            SlideInMenu();
        }
    }

    private void SlideInMenu()
    {
        menuShown = true;
        menuTextObject.SetActive(true);
        barObject.SetActive(true);
        backgroundImage.DOFade(.7f, 1).SetEase(Ease.InOutSine);
        menuGameObject.transform.DOLocalMoveX(settingMoveXEndPos, 1).SetEase(Ease.OutCirc).SetDelay(.5f);
        duckGameObject.transform.DOLocalMove(duckEndPos, 1).SetEase(Ease.OutCirc).SetDelay(.5f);
        audioSource.pitch = 1.5f;
        audioSource.Play();
        SoundManager.instance.FilterOutAudio(true);
    }

    private void SlideOutMenu()
    {
        menuShown = false;
        menuTextObject.SetActive(false);
        barObject.SetActive(false);
        backgroundImage.DOFade(0, 1).SetEase(Ease.InOutSine);
        menuGameObject.transform.DOLocalMoveX(settingsMoveXStartPos, 1).SetEase(Ease.OutCirc);
        duckGameObject.transform.DOLocalMove(duckStartPos, 1).SetEase(Ease.OutCirc);
        SaveDataManager.instance.SaveGameData();
        SoundManager.instance.FilterOutAudio(false);
        audioSource.pitch = 1f;
        audioSource.Play();
    }

    private void CleanUp()
    {
        menuShown = false;
        menuTextObject.SetActive(false);
        barObject.SetActive(false);
         backgroundImage.DOFade(0, 0).SetEase(Ease.InOutSine);
        menuGameObject.transform.DOLocalMoveX(settingsMoveXStartPos, 0).SetEase(Ease.OutCirc);
        duckGameObject.transform.DOLocalMove(duckStartPos, 0).SetEase(Ease.OutCirc);
    }

}
