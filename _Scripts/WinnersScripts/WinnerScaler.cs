using UnityEngine;
using DG.Tweening;
using System;

public class WinnerScaler : MonoBehaviour
{
    public static event Action OnWinnerCall;
    public static event Action OnWinnerCleanUp;

    private float startPosX = 0;
    private float endPosX = 1;

    private void OnEnable()
    {
        GameManager.OnGetWinnerName += DisplayText;
        GameManager.OnResetPlayers += CleanUp;
        GameManager.OnClearPlayers += CleanUp;
    }

    private void OnDisable()
    {
        GameManager.OnGetWinnerName -= DisplayText;
        GameManager.OnResetPlayers -= CleanUp;
        GameManager.OnClearPlayers -= CleanUp;
    }

    private void DisplayText(string nothing)
    {
        transform.DOScaleX(endPosX, 1).SetEase(Ease.InOutBack);
        OnWinnerCall?.Invoke();
    }

    private void CleanUp()
    {
        transform.DOScaleX(startPosX, .5f).SetEase(Ease.InOutBack);
        OnWinnerCleanUp?.Invoke();
    }
}
