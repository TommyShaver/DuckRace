using UnityEngine;
using DG.Tweening;
using TMPro;
public class WinnerNameTextScript : MonoBehaviour
{
    private TextMeshProUGUI winnerText;
    private Transform winnerScale;
    private float startPosX = 0;
    private float endPosX = 1;

    private void Awake()
    {
        winnerText = GetComponent<TextMeshProUGUI>();
        winnerScale = GetComponent<Transform>();
    }

    private void OnEnable()
    {
        GameManager.OnResetPlayers += CleanUpText;
        GameManager.OnClearPlayers += CleanUpText;
        GameManager.OnGetWinnerName += WinnerTextShow;
    }

    private void OnDisable()
    {
        GameManager.OnResetPlayers -= CleanUpText;
        GameManager.OnClearPlayers -= CleanUpText;
        GameManager.OnGetWinnerName -= WinnerTextShow;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        winnerText.text = string.Empty;
        winnerScale.DOScaleX(startPosX, 0).SetEase(Ease.OutSine);
    }

    private void WinnerTextShow(string winnerName)
    {
        winnerText.text = winnerName;
        winnerScale.DOScaleX(endPosX, 1).SetEase(Ease.OutSine);
    }


    private void CleanUpText()
    {
        winnerScale.DOScaleX(startPosX, .5f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            winnerText.text = string.Empty;
        });
    }
}
