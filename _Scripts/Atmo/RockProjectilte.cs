using UnityEngine;
using DG.Tweening;
using System.Collections;

public class RockProjectilte : MonoBehaviour
{
    [SerializeField] GameObject spriteLayer;
    private Tween spinRock;
    private float speedOfRock = 10;
    private SpriteRenderer spriteRenderer;
    private ParticleSystem rockParticleSystem;
    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rockParticleSystem = GetComponentInChildren<ParticleSystem>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 rockSpinRotation = new(0, 0, 100f);
        spinRock = spriteLayer.transform.DOLocalRotate(rockSpinRotation, .2f).SetLoops(-1, LoopType.Restart);
        StartCoroutine(WaitTimer());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speedOfRock * Time.deltaTime * Vector3.right);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DuckInterractionInterface hit = collision.GetComponent<DuckInterractionInterface>();
        if (hit != null)
        {
            hit.DuckInterraction("RockHitDuck", true);
            speedOfRock = 0;
            rockParticleSystem.Play();
            spriteRenderer.sprite = null;
            Debug.Log("RockPrefab: this got called");
        }
    }

    private void BreakRock()
    {
        spinRock.Kill();
        Destroy(gameObject, 1);
    }

    private IEnumerator WaitTimer()
    {
        yield return new WaitForSeconds(10);
        BreakRock();
    }
}
