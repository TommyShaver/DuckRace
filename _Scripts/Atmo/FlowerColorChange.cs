using UnityEngine;

public class FlowerColorChange : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        ChangeColor();
    }

    private void ChangeColor()
    {
        spriteRenderer.flipX = UnityEngine.Random.Range(0, 2) == 0;
        float r = UnityEngine.Random.Range(0.5f, 1.0f);
        float g = UnityEngine.Random.Range(0.5f, 1.0f);
        float b = UnityEngine.Random.Range(0.5f, 1.0f);
        Color newColor = new Color(r, g, b);
        spriteRenderer.color = newColor;
    }
}
