using UnityEngine;

public class DuckFace : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;
    private bool hasLayer;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        DefaultFace();
    }

    public void FaceLayer(int layer)
    {
        if(!hasLayer)
        {
            spriteRenderer.sortingOrder = layer + 1;
            hasLayer = true;
        }
    }

    //changes duck faces over spawned playable duck ------------
    public void DefaultFace()
    {
        spriteRenderer.sprite = sprites[0];
    }
    public void RandomFace(bool isHappy)
    {
        if(!isHappy)
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
}
