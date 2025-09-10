using UnityEngine;
using DG.Tweening;
using System.Collections;
using System;

public class OnDuckRaceLoad : MonoBehaviour
{
    public GameObject spriteMask;
    public static event Action OnUnloadLoading;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(AnimStart());
    }

    private IEnumerator AnimStart()
    {
        OnUnloadLoading?.Invoke();
        yield return new WaitForSeconds(1);
        transform.localScale = new(0, 0, 0);

        transform.DOScale(new Vector3(500, 500, 0), 2).OnComplete(() =>
        {
            SceneLoader.instance.UnloadLoadingScene();
            Destroy(gameObject);
            Destroy(spriteMask);
        });
    }

}
