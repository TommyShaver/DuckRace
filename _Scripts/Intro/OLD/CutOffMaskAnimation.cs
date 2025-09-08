using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutOffMaskAnimation : MonoBehaviour
{
    public static event Action OnAnimationComplete;
    private RectTransform rectTransform;
    private Tween speeeen;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();  
    }

    private void OnEnable()
    {
        LoadingMenuManager.OnButtonClick += AnimStart;
    }
    private void OnDisable()
    {
        LoadingMenuManager.OnButtonClick -= AnimStart;
    }

    private void Start()
    {
        AnimFadeOut(0, 0);
    }

    public void AnimStart()
    {
        speeeen = rectTransform.DORotate(new Vector3(0,0,-180),5).SetLoops(-1,LoopType.Restart);
        rectTransform.DOSizeDelta(Vector2.zero, 2).OnComplete(() =>
        {
            OnAnimationComplete?.Invoke();
            AnimFadeOut(3, 1);
        });
    } 

    public void AnimFadeOut(int time, int delay)
    {
        Vector2 targetSize = new Vector2(15000, 15000);
        rectTransform.DOSizeDelta(targetSize, time).SetDelay(delay).OnComplete(() => {
            speeeen.Kill();
        });
    }
}
