using UnityEngine;
using System.Collections;
using System;
using DG.Tweening;

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
    public GameObject itemBoxSelect;
    public Sprite[] items;
    public SpriteRenderer itemSpriteRenderer;
    private Vector3 resetTrasfrom;

    //Using items ........................
    [Header("Actacul Use Items")]
    public GameObject useTtemTranform;
    public SpriteRenderer useItemRenderer;
    public GameObject rockPrefab;
    public Sprite interbuteSprite;
    public bool usingIntertube;

    //Flames of Rocket......
    public AudioClip[] clipsOfItems;

    private bool animGo;
    private bool canUseItem;
    public bool iHaveItem;
    private int currentLayer = 51;
    private int defaultLayer;
    private int defaultLayer2;

    public AudioSource audioSource;
    public AudioSource audioSFX;

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
        resetTrasfrom = itemBoxSelect.transform.localPosition;
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
        defaultLayer = currentLayer + i;
        defaultLayer2 = currentLayer + i + 1;
    }

    public void SetDefaultLayer()
    {
        gotItemRenderer.sortingOrder = defaultLayer;
        useItemRenderer.sortingOrder = defaultLayer;
        itemSpriteRenderer.sortingOrder = defaultLayer2;
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


    //Get Item Function ---------------------------------------------------------------------
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
            audioSource.clip = clipsOfItems[0];
            audioSource.volume = 1f;
            audioSource.loop = true;
            audioSource.Play();
        });
        animGo = true;
    }

    public void UseItem()
    {
        if (!canUseItem)
            return;
        CleanUpItemBoxes();
        switch (currentItem)
        {
            case ItemType.Rockets:
                ItemLogic(items[0], new Vector3(0, -.2f, 0), Vector3.one, true);
                StartCoroutine(WaitTimer(EndRocket));
                usingIntertube = false;
                duckManager.UseRocket();
                effectsRocket.RocketEffectStart();
                audioSFX.loop = false;
                audioSFX.clip = clipsOfItems[3];
                audioSFX.volume = 1.5f;
                audioSFX.Play();
                break;
            case ItemType.Intertube:
                ItemLogic(interbuteSprite, new Vector3(-.1f, -.2f, 0), new Vector3(3, 3, 1), false);
                audioSFX.loop = false;
                audioSFX.clip = clipsOfItems[1];
                audioSFX.volume = 1f;
                audioSFX.Play();
                usingIntertube = true;
                break;
            case ItemType.Rock:
                Instantiate(rockPrefab, transform.position + new Vector3(1.7f, 0, 0), Quaternion.identity);
                usingIntertube = false;
                break;
        }
        gotItemRenderer.sprite = null;
        itemBoxSelect.transform.localPosition = resetTrasfrom;
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
        gotItemRenderer.sprite = gotItemBoxSprite;
        Debug.Log(items[(int)currentItem]);
        audioSource.Stop();
        audioSource.loop = false;
    }

    private IEnumerator ItemAnim()
    {
        audioSource.Play();
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
        itemSpriteRenderer.sprite = items[(int)currentItem];
    }

    //Item Clean up's -----------------------------------------------------------------
    private void EndRocket()
    {
        duckManager.GetOutOfRocket();
        effectsRocket.RocketEffectStop();
        useItemRenderer.sprite = null;
    }
    public void IntertubeGoPop()
    {
        usingIntertube = false;
        audioSFX.loop = false;
        audioSFX.clip = clipsOfItems[2];
        audioSFX.volume = .7f;
        audioSFX.Play();
    }
}
