using DG.Tweening;
using System.Drawing;
using System.Xml.Linq;
using UnityEngine;

public class DuckHats : MonoBehaviour
{
    public Vector3[] hatPos;
    public Sprite[] hats;

    private float hatangle = 30f;

    private SpriteRenderer hatRenderer;
    private bool iHaveALayer;
    private int currerntLayer = 55;

    private void Awake()
    {
       hatRenderer = GetComponent<SpriteRenderer>();     
    }
   
    public void ChangeSortingLayer(int i)
    {
        if (!iHaveALayer)
        {
            hatRenderer.sortingOrder = i + currerntLayer;
            iHaveALayer = true;
        }
    }
    public void DuckChangedLayer(int i)
    {
        //So when the ducks change levels the sort order travels with it.
        hatRenderer.sortingOrder += i;
    }

    public void FlipSprite(bool isTrue)
    {
        hatRenderer.flipX = isTrue;
    }

    public void ChangeHat(string hat)
    {
        switch (hat)
        {
            case "bow":
                hatRenderer.sprite = hats[0];
                ChangeTransform(hatPos[0], hatangle, Vector2.one);
                break;
            case "cowboy":
                hatRenderer.sprite = hats[1];
                ChangeTransform(hatPos[1], 0, new Vector2(2, 2));
                break;
            case "cheif":
                hatRenderer.sprite = hats[2];
                ChangeTransform(hatPos[2], 0, new Vector2(1.2f, 1.2f));
                break;
            case "navi":
                hatRenderer.sprite = hats[3];
                ChangeTransform(hatPos[3], 0, Vector2.one);
                break;
            case "santa":
                hatRenderer.sprite = hats[4];
                ChangeTransform(hatPos[4], hatangle, Vector2.one);
                break;
            case "party":
                hatRenderer.sprite = hats[UnityEngine.Random.Range(5, 8)];
                ChangeTransform(hatPos[5], 0, Vector2.one);
                break;
            case "witch":
                hatRenderer.sprite = hats[9];
                ChangeTransform(hatPos[9], hatangle, Vector2.one);
                break;
            case "warlock":
                hatRenderer.sprite = hats[10];
                ChangeTransform(hatPos[10], hatangle, Vector2.one);
                break;
            case "straw":
                hatRenderer.sprite = hats[11];
                ChangeTransform(hatPos[11], 0, new Vector2(2, 2));
                break;
            case "none":
                ChangeTransform(Vector3.zero, 0, Vector3.one);
                hatRenderer.sprite = null;
                break;
        }
    }

    private void ChangeTransform(Vector3 postion, float zRotation, Vector2 size)
    {
        transform.localPosition = postion;
        transform.DOLocalRotate(new Vector3(0,0,zRotation),0);
        transform.localScale = size;
    }
}
