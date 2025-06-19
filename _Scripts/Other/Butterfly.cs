using UnityEngine;

public class Butterfly : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Animator animator;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetColor();
        GetAnimation();
    }

    private void GetColor()
    {
        float r = Random.Range(0.0f, 1.0f);
        float g = Random.Range(0.0f, 1.0f);
        float b = Random.Range(0.0f, 1.0f);
        Color newColor = new Color(r, g, b);
        sprite.color = newColor;
    }

    private void GetAnimation()
    {
        int i = Random.Range(0, 2);
        float speed = Random.Range(.7f, 1.5f);
        animator.SetFloat("speed", speed);
        if(speed <= 1.3f)
        {
            animator.SetBool("backwards", true);
        }
        if (i == 0)
        {
            animator.Play("ButterflyAnim");
        }
        else
        {
            animator.Play("ButterflyAnim2");
        }
    }
}
