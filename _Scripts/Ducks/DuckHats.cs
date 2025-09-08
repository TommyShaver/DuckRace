using System.Collections;
using System.Threading;
using DG.Tweening;
using UnityEngine;

public class DuckHats : MonoBehaviour
{
    public Vector3[] hatPos;
    public Sprite[] hats;

    private float hatangle = 30f;

    private SpriteRenderer hatRenderer;
    private bool iHaveALayer;
    private int currerntLayer = 55;
    private int defaultLayer;
    private string whichHat;
    private bool hasColor;

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
            defaultLayer = i + currerntLayer;
        }
    }

    public void SetDefaultLayer()
    {
        hatRenderer.sortingOrder = defaultLayer;
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

    //? NOT THIS ONE ...........................................................
    public void ChangeHat(string hat)
    {
        hatRenderer.flipX = true;
        if (hat != "bow")
        {
            hatRenderer.color = Color.white;   
        }
        switch (hat)
        {
            case "bow":
                hatRenderer.sprite = hats[0];
                ChangeTransform(hatPos[0], hatangle, Vector2.one);
                hatRenderer.flipX = false;
                GetColor();
                hasColor = true;
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
        whichHat = hat;
    }
    //? NOT THIS ONE ...................................................................

    //! CHANGE THIS ONE! ?????????????????????????????????????????????????????????????
    public void ChangeHatOnTant(bool active)
    {
        if (!active)
        {
            StartCoroutine(WaitPlease());
        }

        switch (whichHat)
        {
            case "bow":
                TauntHatLogic(false, new Vector3(-0.06f, -0.01f, 0), -30);
                break;
            case "cowboy":
                TauntHatLogic(false, new Vector3(0.03f, -.26f, 0), 0);
                break;
            case "cheif":
                TauntHatLogic(false, new Vector3(-.02f, 0, 0), 0);
                break;
            case "navi":
                //Do Nothing
                break;
            case "santa":
                TauntHatLogic(false, new Vector3(-.01f, .12f, 0), -30);
                break;
            case "party":
                TauntHatLogic(false, new Vector3(0, .15f, 0), 0);
                break;
            case "witch":
                TauntHatLogic(false, new Vector3(0, .15f, 0), -30);
                break;
            case "warlock":
                TauntHatLogic(false, new Vector3(0, .15f, 0), -30);
                break;
            case "straw":
                TauntHatLogic(false, new Vector3(0.03f, -.22f, 0), 0);
                break;
            case "none":
                //Do Nothing
                break;
        }
    }

    private void TauntHatLogic(bool flipX, Vector3 tauntPos, float zRotation)
    {
        hatRenderer.flipX = flipX;
        transform.localPosition = tauntPos;
        transform.localEulerAngles = new Vector3(0f, 0f, zRotation);
    }


    //! CHANGE THIS ONE! ?????????????????????????????????????????????????????????????

    private void ChangeTransform(Vector3 postion, float zRotation, Vector2 size)
    {
        Debug.Log(postion + ":" + zRotation + ":" + size);
        transform.localPosition = postion;
        transform.localEulerAngles = new Vector3(0f, 0f, zRotation);
        transform.localScale = size;
    }

    public string CurrentHat()
    {
        string currentHat = whichHat;
        if (currentHat == string.Empty)
        {
            currentHat = "none";
        }
        return currentHat;
    }

    private IEnumerator WaitPlease()
    {
        yield return new WaitForSeconds(.05f);
        ChangeHat(whichHat);
    }
    private void GetColor()
    {
        if (hasColor)
            return;

        float r = UnityEngine.Random.Range(0.0f, 1.0f);
        float g = UnityEngine.Random.Range(0.0f, 1.0f);
        float b = UnityEngine.Random.Range(0.0f, 1.0f);
        Color newColor = new Color(r, g, b);
        hatRenderer.color = newColor;
    }
}
