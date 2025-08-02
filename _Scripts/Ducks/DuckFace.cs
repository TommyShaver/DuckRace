using UnityEngine;

public class DuckFace : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;
    private bool hasLayer;
    private int currentLayer = 51;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        DefaultFace();
    }

    public void FaceLayer(int layer)
    {
        if (!hasLayer)
        {
            spriteRenderer.sortingOrder = layer + currentLayer;
            hasLayer = true;
        }
    }

    public void DuckChangedLayer(int i)
    {
        //So when the ducks change levels the sort order travels with it.
        spriteRenderer.sortingOrder += i;
    }

    //changes duck faces over spawned playable duck ------------
    public void DefaultFace()
    {
        spriteRenderer.sprite = sprites[0];
    }

    public void RandomFace(bool isHappy)
    {
        if (!isHappy)
        {
            int randomNumber = Random.Range(1, 3);
            spriteRenderer.sprite = sprites[randomNumber];
        }
        else
        {
            int randomNumber = Random.Range(4, 5);
            spriteRenderer.sprite = sprites[randomNumber];
        }
    }

    public void FlipSprite(bool isTrue)
    {
        spriteRenderer.flipX = isTrue;
        if (!isTrue)
        {
            RandomFace(UnityEngine.Random.Range(0, 2) == 0);
        }
        else
        {
            DefaultFace();
        }
    }
}
