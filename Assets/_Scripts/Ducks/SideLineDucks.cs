using UnityEngine;

public class SideLineDucks : MonoBehaviour
{

    //These ducks are atmo pieces nno really game items 
    //Change color and animation;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private string[] animaitonSelection = { "DuckAnim", "DuckSpin", "DuckJump" };

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    
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
        spriteRenderer.color = newColor;
    }

    private void GetAnimation()
    {
        animator.Play(animaitonSelection[Random.Range(0, animaitonSelection.Length)]);
        animator.SetFloat("ChangeSpeed", Random.Range(.5f, 1.2f));
    }
}
