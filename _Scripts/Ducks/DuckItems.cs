using UnityEngine;
using System.Collections;
using System;
using DG.Tweening;
using System.Globalization;

public class DuckItems : MonoBehaviour
{

    private DuckManager duckManager;
    private DuckEffectsRocket effectsRocket;
    //Got item box........................
    [Header("Got Item Box")]
    public GameObject gotItemBox;
    public SpriteRenderer gotItemRenderer;
    public Sprite gotItemBoxSprite;

    //sprite of items ...................
    [Header("Sprite Renderer Of Item Animaiton")]
    public Sprite[] items;
    public SpriteRenderer itemSpriteRenderer;


    //Using items ........................
    [Header("Actacul Use Items")]
    public GameObject useTtemTranform;
    public SpriteRenderer useItemRenderer;
    public GameObject rockPrefab;
    public Sprite interbuteSprite;
    public bool usingIntertube;

    //Flames of Rocket......
    

    private bool animGo;
    private bool canUseItem;
    private bool iHaveItem;
    private int currentLayer = 52;
  

    private ItemType currentItem;
    public enum ItemType
    {
        Rockets,
        Intertube,
        Rock
    };

    private void Awake()
    {
        duckManager = GetComponentInParent<DuckManager>();
        effectsRocket = GetComponentInChildren<DuckEffectsRocket>();
    }

    private void Start()
    {
        canUseItem = false;
        useItemRenderer.sprite = null;
        CleanUpItemBoxes();
        gotItemRenderer.sprite = null;
    }

    public void CleanUpItemBoxes()
    {
        canUseItem = false;
        usingIntertube = false;
        itemSpriteRenderer.sprite = null;
        useItemRenderer.sprite = null;
        gotItemBox.transform.localScale = Vector3.zero;
        iHaveItem = false;
    }

    public void SpawnDuckItems(int i)
    {
        gotItemRenderer.sortingOrder = currentLayer + i;
        itemSpriteRenderer.sortingOrder = currentLayer + i + 1;
        useItemRenderer.sortingOrder = currentLayer + i;
        effectsRocket.SpawnEffectItems(i);
    }

    public void DuckChangedLayer(int i)
    {
        //So when the ducks change levels the sort order travels with it.
        gotItemRenderer.sortingOrder += i;
        itemSpriteRenderer.sortingOrder += i;
        useItemRenderer.sortingOrder += i;
        effectsRocket.DuckChangedLayer(i);
    }

    public void ResetCleanUp()
    {
        canUseItem = false;
        useItemRenderer.sprite = null;
        CleanUpItemBoxes();
        gotItemRenderer.sprite = null;
        StopAllCoroutines();
        effectsRocket.RocketEffectStop();
    }

    public void GetItem()
    {
        if (iHaveItem)
            return;
        gotItemRenderer.sprite = gotItemBoxSprite;
        Vector3 boxSize = new(1.5f, 1.5f, 0);
        gotItemBox.transform.DOScale(boxSize, .5f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            StartCoroutine(WaitTimer(SelectItem));
            StartCoroutine(ItemAnim());
        });
        animGo = true;
    }

    public void UseItem()
    {
        if (!canUseItem)
            return;
        CleanUpItemBoxes();
        currentItem = 0;
        switch (currentItem)
        {
            case ItemType.Rockets:
                ItemLogic(items[0], new Vector3(0, -.2f, 0), Vector3.one, true);
                StartCoroutine(WaitTimer(EndRocket));
                usingIntertube = false;
                duckManager.UseRocket();
                effectsRocket.RocketEffectStart();
                break;
            case ItemType.Intertube:
                ItemLogic(interbuteSprite, new Vector3(-.1f, -.2f, 0), new Vector3(3, 3, 1), false);
                break;
            case ItemType.Rock:
                Instantiate(rockPrefab, transform.position + new Vector3(1.7f, 0, 0), Quaternion.identity);
                usingIntertube = false;
                break;
        }
        gotItemRenderer.sprite = null;
    }


    private void ItemLogic(Sprite sprite, Vector3 itemPos, Vector3 size, bool flip)
    {
        useItemRenderer.sprite = sprite;
        useTtemTranform.transform.localPosition = itemPos;
        useTtemTranform.transform.localRotation = Quaternion.Euler(new Vector3(-15, 0, 0));
        useTtemTranform.transform.localScale = size;
        useItemRenderer.flipX = flip;
    }
   
    //This set the animation and relases it when the timer is up.
    private IEnumerator WaitTimer(Action funciton)
    {
        yield return new WaitForSeconds(3);
        funciton.Invoke();
    }
    private void SelectItem()
    {
        animGo = false;
        canUseItem = true;
        iHaveItem = true;
        itemSpriteRenderer.sprite = items[(int)currentItem];
        gotItemRenderer.sprite = gotItemBoxSprite;
    }
    private void EndRocket()
    {
        CleanUpItemBoxes();
        duckManager.GetOutOfRocket();
        effectsRocket.RocketEffectStop();
        useItemRenderer.sprite = null;
    }

    private IEnumerator ItemAnim()
    {
        currentItem = (ItemType)UnityEngine.Random.Range(0, items.Length);
        Debug.Log(currentItem);
        while (animGo)
        {
            for (int i = 0; i < items.Length; i++)
            {
                itemSpriteRenderer.sprite = items[i];
                yield return new WaitForSeconds(.1f);
                gotItemRenderer.sprite = gotItemBoxSprite;
            }
        }
    }
}
