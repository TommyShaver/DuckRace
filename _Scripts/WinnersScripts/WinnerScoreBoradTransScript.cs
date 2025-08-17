using UnityEngine;
using DG.Tweening;
public class WinnerScoreBoradTransScript : MonoBehaviour
{
    public GameObject leftBar;
    public GameObject rightBar;

    public GameObject scoreBorad;

    private void OnEnable()
    {
        GameManager.OnShowScoreBorad += AnimationStart;
        GameManager.OnResetPlayers += AniamtionCleanUp;
        GameManager.OnClearPlayers += AniamtionCleanUp;
    }

    private void OnDisable()
    {
        GameManager.OnShowScoreBorad -= AnimationStart;
        GameManager.OnResetPlayers -= AniamtionCleanUp;
        GameManager.OnClearPlayers -= AniamtionCleanUp;
    }

    private void Start()
    {
        leftBar.transform.DOLocalMoveX(-5000, 0);
        rightBar.transform.DOLocalMoveX(5000, 0);
        scoreBorad.SetActive(false);
    }

    private void AnimationStart()
    {
        leftBar.transform.DOLocalMoveX(0, 1f).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            scoreBorad.SetActive(true);
            leftBar.transform.DOLocalMoveX(5000, 1f).SetEase(Ease.InOutBack).SetDelay(.5f);
            ScoreboradManager.instance.ShowScoreOnDraw();
        });
        rightBar.transform.DOLocalMoveX(0, .5f).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            rightBar.transform.DOLocalMoveX(-5000, 1f).SetEase(Ease.InOutBack).SetDelay(.5f);
        });
    }

    private void AniamtionCleanUp()
    {
        leftBar.transform.DOLocalMoveX(0, 1f).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            scoreBorad.SetActive(false);
            leftBar.transform.DOLocalMoveX(5000, 1f).SetEase(Ease.InOutBack).SetDelay(.5f);
        });
        rightBar.transform.DOLocalMoveX(0, .5f).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            rightBar.transform.DOLocalMoveX(-5000, 1f).SetEase(Ease.InOutBack).SetDelay(.5f);
        });
    }
}
