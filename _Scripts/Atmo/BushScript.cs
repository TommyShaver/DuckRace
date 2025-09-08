using UnityEngine;

public class BushScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        ChangeScaleColor();
    }

    private void ChangeScaleColor()
    {
        spriteRenderer.flipX = NumberGen(0, 2) == 2;
        transform.localScale = new(NumberGen(4.7f,5.2f), NumberGen(4.7f,5.2f), 1);
    }

    private float NumberGen(float inputA, float inputB)
    {
        float pickedNumber = UnityEngine.Random.Range(inputA, inputB);
        return pickedNumber;
    }
}
