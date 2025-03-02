using UnityEngine;
using DG.Tweening;

public class DuckColor : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool iHaveALayer = false;
    private Tween rotationTween;

    private void Awake()
    {
       spriteRenderer = GetComponent<SpriteRenderer>(); 
    }

    private void Start()
    {
        GetColor();
    }

    //Get Random Color per duck. ---------------------------------------------------------------------------------------------
    private void GetColor()
    {
        float r = Random.Range(0.0f, 1.0f);
        float g = Random.Range(0.0f, 1.0f);
        float b = Random.Range(0.0f, 1.0f);
        Color newColor = new Color(r,g,b);
        spriteRenderer.color = newColor;
    }

    //I had to do this so each duck doesn't overlap the DuckManager does the math logic. --------------------------------------
    public void ChangeSortingLayer(int i)
    {
        if (!iHaveALayer) 
        {
            spriteRenderer.sortingOrder = i;
            iHaveALayer = true;
        }
    }


    //Rotate duck, I ened up doing it here so it didn't mess with my ducks moving towards goal. ------------------------------- 
    public void SpriteRoation(bool isMoving)
    {
        if(isMoving)
        {
            rotationTween = transform.DOLocalRotate(new Vector3(0,0,4f),1).SetEase(Ease.InOutBack).SetLoops(-1, LoopType.Yoyo);
            rotationTween.Play();
            Debug.Log("Duck is Rotating");
        }
    }

    public void KillTween()
    {
        rotationTween.Kill();
        transform.eulerAngles = new Vector3(0, 0, 0);
    }
}
