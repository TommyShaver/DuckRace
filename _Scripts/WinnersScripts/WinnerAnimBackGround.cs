using UnityEngine;
using DG.Tweening;

public class WinnerAnimBackGround : MonoBehaviour
{
    public GameObject[] squares;
    public Vector3[] endPos;

    private Vector3[] startPos;

    private void OnEnable()
    {
        GameManager.OnWinner += WinnerAnimStart;
        GameManager.OnResetPlayers += CleanUpWinnerBackGround;
        GameManager.OnClearPlayers += CleanUpWinnerBackGround;
    }

    private void OnDisable()
    {
        GameManager.OnWinner -= WinnerAnimStart;
        GameManager.OnResetPlayers += CleanUpWinnerBackGround;
        GameManager.OnClearPlayers += CleanUpWinnerBackGround;
    }

    private void Start()
    {
        startPos = new Vector3[squares.Length];
        for (int i = 0; i < squares.Length; i++)
        {
            SpriteRenderer sr = squares[i].GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.DOFade(0, 0);
            }

            startPos[i] = squares[i].transform.position;
        }
    }

    private void WinnerAnimStart()
    {
        Debug.Log("we made it to the winner call");
        for (int i = 0; i < squares.Length; i++)
        {
            SpriteRenderer sr = squares[i].GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.DOFade(1, .5f).SetEase(Ease.InOutSine);

            }
            squares[i].transform.DOLocalMove(endPos[i], .5f).SetEase(Ease.InOutSine).SetDelay(.2f);
        }
    }

    private void CleanUpWinnerBackGround()
    {
        Debug.Log("we made it to the winner clean up call");
        for (int i = 0; i < squares.Length; i++)
        {
            SpriteRenderer sr = squares[i].GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.DOFade(0, 0);
            }
            squares[i].transform.position = startPos[i];
        }
    }
}
