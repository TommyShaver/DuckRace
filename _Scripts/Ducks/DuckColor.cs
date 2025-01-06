using UnityEngine;

public class DuckColor : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool iHaveALayer = false;

    private void Awake()
    {
       spriteRenderer = GetComponent<SpriteRenderer>(); 
    }

    private void Start()
    {
        GetColor();
    }
    private void GetColor()
    {
        float r = Random.Range(0.0f, 1.0f);
        float g = Random.Range(0.0f, 1.0f);
        float b = Random.Range(0.0f, 1.0f);
        Color newColor = new Color(r,g,b);
        spriteRenderer.color = newColor;
    }

    public void ChangeSortingLayer(int i)
    {
        if (!iHaveALayer) 
        {
            spriteRenderer.sortingOrder = i;
            iHaveALayer = true;
        }
    }
}
