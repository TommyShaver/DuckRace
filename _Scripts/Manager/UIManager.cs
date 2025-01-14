using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject BlackOverlay;
    public TMP_InputField switchUserName;
    public TextMeshProUGUI placeHolderText; // Show current user name
    public RectTransform UIMenuRectTracs;
    private bool isMenuOpen;
    private bool canMove = true;

    public static event Action OnClearPlayers;
    public static event Action OnResetPlayers;

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
        switchUserName.onEndEdit.AddListener(OnEndEdit);
    }

    //Main Menu Logic --------------------------------------------------------------------------------------
    public void OpenMenu()
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
                GameManager.instance.TimeMagament(0);
            }
            else
            {
                GameManager.instance.TimeMagament(1);
                UIMenuRectTracs.DOAnchorPosX(500, .5f, false).SetEase(Ease.InOutBack).SetUpdate(true);
                isMenuOpen = false;
                BlackOverlay.SetActive(false);
            }
        }
    }

    private void OnEndEdit(string inputText) //Change username
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            TwitchManager.instance.SwitchTwitchUserName(inputText);
            placeHolderText.text = inputText + " connected";
        }
    }

    public void ResetBorad()
    {
        OnResetPlayers?.Invoke();
    }

    public void ClearPlayer()
    {
        SpawnManager.Instance.ResetSpawnCount();
        OnClearPlayers?.Invoke(); //Duck Manager
    }

    private IEnumerator WaitForAnimation()
    {
        yield return new WaitForSecondsRealtime(.5f);
        canMove = true;
    }
}
