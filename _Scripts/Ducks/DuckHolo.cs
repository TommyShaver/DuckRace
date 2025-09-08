using UnityEngine;

public class DuckHolo : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private int currentLayer = 51;
    private int defaultLayer;
    private bool iHaveAName = false;
    private bool isHolo;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    public void ChangeSortingLayer(int i)
    {
        if (!iHaveAName)
        {
            spriteRenderer.sortingOrder = i + currentLayer;
            iHaveAName = true;
            DuckIsHolo();
            defaultLayer = i + currentLayer;
        }
    }

    public void SetDefaultLayer()
    {
        spriteRenderer.sortingOrder = defaultLayer;
    }
    public void DuckChangedLayer(int i)
    {
        //So when the ducks change levels the sort order travels with it.
        spriteRenderer.sortingOrder += i;
    }

    private void DuckIsHolo()
    {
        int randomNumber = UnityEngine.Random.Range(1, 101);
        if (FivePercentChance())
        {
            spriteRenderer.enabled = true;
            isHolo = true;
            audioSource.Play();
        }
        else
        {
            spriteRenderer.enabled = false;
            isHolo = false;
        }

    }

    private bool FivePercentChance()
    {
        int roll = UnityEngine.Random.Range(1, 101);
        return roll < 5;
    }


    public void FlipMaterialText(bool flipX)
    {
        if (flipX)
        {
            transform.localScale = new(-1, 1, 1);
        }
        else
        {
            transform.localScale = new(1, 1, 1);
        }
    }

    public bool IsDuckHolo()
    {
        return isHolo;
    }
}
