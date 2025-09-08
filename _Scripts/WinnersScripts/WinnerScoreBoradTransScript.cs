using UnityEngine;
using DG.Tweening;
public class WinnerScoreBoradTransScript : MonoBehaviour
{
    public GameObject leftBar;
    public GameObject rightBar;
    public GameObject bigStar1;
    public GameObject bigStar2;
    public GameObject bigStar3;
    public GameObject smallStar1;
    public GameObject smallStar2;
    public GameObject smallStar3;

    public GameObject scoreBorad;
    public AudioSource leftAudioSource;
    public AudioSource rightAudioSource;

    private void OnEnable()
    {
        GameManager.OnShowScoreBorad += AnimationStart;
        GameManager.OnResetPlayers += AniamtionCleanUp;
    }

    private void OnDisable()
    {
        GameManager.OnShowScoreBorad -= AnimationStart;
        GameManager.OnResetPlayers -= AniamtionCleanUp;
    }

    private void Start()
    {
        leftBar.transform.DOLocalMoveX(-5000, 0);
        rightBar.transform.DOLocalMoveX(5000, 0);
        scoreBorad.SetActive(false);
        //StarsSpeeeen();

    }

    public void AnimationStart()
    {
        leftAudioSource.Play();
        rightAudioSource.PlayDelayed(.2f);
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

        //I am being lazy I just want it done. 
        bigStar1.transform.DOLocalMoveX(-1274, RandomFloat());
        bigStar2.transform.DOLocalMoveX(1274, RandomFloat());
        bigStar3.transform.DOLocalMoveX(-1274, RandomFloat());
        bigStar1.transform.DOLocalMoveX(-1274, RandomFloat());
        smallStar1.transform.DOLocalMoveX(1074, RandomFloat());
        smallStar2.transform.DOLocalMoveX(1074, RandomFloat());
        smallStar3.transform.DOLocalMoveX(-1074, RandomFloat());
    }

    public void AniamtionCleanUp()
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


        bigStar1.transform.DOLocalMoveX(1274, RandomFloat());
        bigStar2.transform.DOLocalMoveX(-1274, RandomFloat());
        bigStar3.transform.DOLocalMoveX(1274, RandomFloat());
        bigStar1.transform.DOLocalMoveX(1274, RandomFloat());
        smallStar1.transform.DOLocalMoveX(-1074, RandomFloat());
        smallStar2.transform.DOLocalMoveX(-1074, RandomFloat());
        smallStar3.transform.DOLocalMoveX(1074, RandomFloat());
        leftAudioSource.Play();
        rightAudioSource.PlayDelayed(.2f);
        
    }

    private void StarsSpeeeen()
    {
        Transform[] stars = { bigStar1.transform, bigStar2.transform, bigStar3.transform, smallStar1.transform, smallStar2.transform, smallStar3.transform };
        foreach (var star in stars)
        {
            star.transform.DORotate(new Vector3(0, 0, 360), RandomNumber(), RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear);
        }
    }
    
    private int RandomNumber()
    {
        int randNum = UnityEngine.Random.Range(1, 4);
        return randNum;
    }

    private float RandomFloat()
    {
        float randFloat = UnityEngine.Random.Range(.5f, 2f);
        return randFloat;
    }
}
