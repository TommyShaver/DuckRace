using UnityEngine;

public class DuckFace : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        DefaultFace();
    }

    public void DefaultFace()
    {
        spriteRenderer.sprite = sprites[0];
    }
    public void RandomFace()
    {
        int randomNumber = Random.Range(1, sprites.Length);
        spriteRenderer.sprite = sprites[randomNumber];  
    }
}
