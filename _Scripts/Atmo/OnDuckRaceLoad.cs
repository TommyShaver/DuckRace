using UnityEngine;
using DG.Tweening;
using System.Collections;

public class OnDuckRaceLoad : MonoBehaviour
{
    public GameObject spriteMask;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(AnimStart());
    }

    private IEnumerator AnimStart()
    {
        SceneLoader.instance.UnloadLastScene(SceneLoader.LoadedScene.LoadingScene);
        Debug.Log("We tried to unload scene");
        yield return new WaitForSeconds(1);
        transform.localScale = new(0, 0, 0);

        transform.DOScale(new Vector3(500, 500, 0), 2).OnComplete(() =>
        {
            Destroy(gameObject);
            Destroy(spriteMask);
        });
    }

}
