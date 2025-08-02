using UnityEngine;

public class BoulderOpstical : MonoBehaviour
{
    [SerializeField] SpriteRenderer waveRendererLeft;
    [SerializeField] SpriteRenderer waveRendererRight;
    private SpriteRenderer rockspriteRenderer;

    private void Awake()
    {
        rockspriteRenderer = GetComponent<SpriteRenderer>();
        waveRendererLeft = GetComponentInChildren<SpriteRenderer>();
        waveRendererRight = GetComponentInChildren<SpriteRenderer>();
    }
    private void Start()
    {
        rockspriteRenderer.flipX = UnityEngine.Random.Range(0, 2) == 0;
        int yPos = Mathf.RoundToInt(transform.position.y);
        SetLayer(yPos);
    }

    private void SetLayer(int pos)
    {
        int baseSortingOrder = 50;
        int offset = -pos;
        rockspriteRenderer.sortingOrder = baseSortingOrder + offset;
        waveRendererLeft.sortingOrder = baseSortingOrder + offset;
        waveRendererRight.sortingOrder = baseSortingOrder + offset;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DuckInterractionInterface hit = collision.GetComponent<DuckInterractionInterface>();
        if (hit != null)
        {
            hit.DuckInterraction("Boulder", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        DuckInterractionInterface hit = collision.GetComponent<DuckInterractionInterface>();
        if (hit != null)
        {
            hit.DuckInterraction("Boulder", false);
        }
    }

    private void DestoryRock()
    {
        Destroy(gameObject);
    }

    
}
