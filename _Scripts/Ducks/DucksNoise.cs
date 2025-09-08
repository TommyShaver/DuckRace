using UnityEngine;

public class DucksNoise : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool iHaveALayer;
    private int currerntLayer = 52;
    private int defaultLayer;
    private Vector3 spawnPos;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void Start()
    {
        spawnPos = transform.localPosition;
    }

    public void ChangeSortingLayer(int i)
    {
        if (!iHaveALayer)
        {
            spriteRenderer.sortingOrder = i + currerntLayer;
            iHaveALayer = true;
            defaultLayer = i + currerntLayer;
        }
    }
    public void DuckChangedLayer(int i)
    {
        //So when the ducks change levels the sort order travels with it.
        spriteRenderer.sortingOrder += i;
    }
    public void SetDefaultLayer()
    {
        spriteRenderer.sortingOrder = defaultLayer;
    }

    public void FlipSprite(bool isTrue)
    {
        spriteRenderer.flipX = isTrue;

        if (!isTrue)
        {
            transform.localPosition = Vector3.zero;
        }
        else
        {
            transform.localPosition = spawnPos;
        }
    }
}
