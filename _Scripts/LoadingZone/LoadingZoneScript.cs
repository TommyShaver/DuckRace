using UnityEngine;
using DG.Tweening;

public class LoadingZoneScript : MonoBehaviour
{
    public static LoadingZoneScript instance { get; private set; }
    public SpriteRenderer backgroundSpriteRenderer;
    public SpriteRenderer duckSpriteRenderer;
    public SpriteRenderer eyeSpriteRenderer;
    public Animator duckAnimation;

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
        backgroundSpriteRenderer.DOFade(0, 0);
        duckSpriteRenderer.DOFade(0, 0);
        eyeSpriteRenderer.DOFade(0, 0);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OnLoadScene();
    }

    private void OnLoadScene()
    {
        int randNum;
        backgroundSpriteRenderer.DOFade(1, .5f).OnComplete(() =>
        {
            randNum = UnityEngine.Random.Range(0, 2);
            duckAnimation.SetFloat("ChangeSpeed", 1);
            duckSpriteRenderer.DOFade(1, .5f);
            eyeSpriteRenderer.DOFade(1, .5f);
            if (randNum == 0)
            {
                duckAnimation.Play("DuckSpin");
            }
            else
            {
                duckAnimation.Play("DuckJump");
            }
        });
    }

    public void CleanUpLoadingZone()
    {
        duckSpriteRenderer.DOFade(0, .5f);
        eyeSpriteRenderer.DOFade(0, .5f).OnComplete(() =>
        {
            backgroundSpriteRenderer.DOFade(0, 0.5f);
        });
    }
}
