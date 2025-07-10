using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class DuckColor : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool iHaveALayer = false;
    private Tween rotationTween;
    private int currentLayer = 50;
    private float cycDuration = 3.25f;
    private Tween colorTween;

    public Color[] colors;  

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
            spriteRenderer.sortingOrder = currentLayer + i;
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

    public void ChatMemberChangedColor(string color)
    {
        colorTween.Kill();
            switch(color)
            {
                case "red":
                    spriteRenderer.color = Color.red;
                    break;
                case "blue":
                    spriteRenderer.color = Color.blue;
                    break;
                case "green":
                    spriteRenderer.color = Color.green;
                    break;
                case "purple":
                    spriteRenderer.color = colors[0];
                    break;
                case "yellow":
                    spriteRenderer.color = Color.yellow;
                    break;
                case "orange":
                    spriteRenderer.color = colors[1];
                    break;
                case "pink":
                    spriteRenderer.color = colors[2];
                    break;
                case "white":
                    spriteRenderer.color = Color.white;
                    break;
                case "black":
                    spriteRenderer.color = Color.black;
                    break;
                case "brown":
                    spriteRenderer.color = colors[3];
                    break;
                case "random":
                    GetColor();
                    break;
                case "rainbow":
                    RainBowDuck();
                    break;
        }
    }

    private void RainBowDuck()
    {
        colorTween = DOTween.To(() => 0f, h => {
            Color color = Color.HSVToRGB(h, 1f, 1f);
            spriteRenderer.color = color;
        }, 1f, cycDuration).SetLoops(-1,LoopType.Restart).SetEase(Ease.Linear);
    }
    public void DuckChangedLayer(int i)
    {
        //So when the ducks change levels the sort order travels with it.
        spriteRenderer.sortingOrder += i;
    }
}
