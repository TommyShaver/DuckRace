using UnityEngine;
using DG.Tweening;

public class IntroDuckAnimation : MonoBehaviour
{
    private float duckDis = 20;
    private Tween introDuckAnim;
    private Tween rotationLoop;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        introDuckAnim = transform.DOLocalMoveX(duckDis, 10).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Restart);
        rotationLoop = transform.DOLocalRotate(new Vector3(0,0,-5), 1).SetEase(Ease.InOutBack).SetLoops(-1,LoopType.Yoyo);
    }

   

    public void UnloadDuck()
    {
        introDuckAnim.Kill();
        rotationLoop.Kill();
    }
}
