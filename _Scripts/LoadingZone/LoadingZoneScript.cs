using UnityEngine;
using DG.Tweening;
using System.Collections;

public class LoadingZoneScript : MonoBehaviour
{
    public GameObject m_Camera;
    public GameObject duckGameboject;
    public SpriteRenderer backgroundSpriteRenderer;
    public SpriteRenderer duckSpriteRenderer;
    public SpriteRenderer eyeSpriteRenderer;
    public Animator duckAnimation;

    private void Awake()
    {
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
            TryUnload();
        });
    }

    private void TryUnload()
    {
        SceneLoader.instance.UnloadLastScene(SceneLoader.LoadedScene.OpeningScene);
        SceneLoader.instance.UnloadLastScene(SceneLoader.LoadedScene.FirstTimeLoad);
        SceneLoader.instance.AdvanceToNextScene(SceneLoader.LoadedScene.DuckRace);
        StartCoroutine(CleanUpLoadingZone());
    }

    public IEnumerator CleanUpLoadingZone()
    {
        yield return new WaitForSeconds(1);
        duckSpriteRenderer.DOFade(0, .5f);
        eyeSpriteRenderer.DOFade(0, .5f).OnComplete(() =>
        {
            backgroundSpriteRenderer.DOFade(0, 0.5f);
        });
    }

}
