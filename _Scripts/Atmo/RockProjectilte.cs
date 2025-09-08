using UnityEngine;
using DG.Tweening;
using System.Collections;

public class RockProjectilte : MonoBehaviour
{
    [SerializeField] GameObject rockPrefab;
    [SerializeField] GameObject spriteLayer;
    private Tween spinRock;
    private float speedOfRock;
    private SpriteRenderer spriteRenderer;
    private ParticleSystem rockParticleSystem;
    private AudioSource audioSource;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rockParticleSystem = GetComponentInChildren<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speedOfRock = UnityEngine.Random.Range(10, 15);
        Vector3 rockSpinRotation = new(0, 0, 100f);
        spinRock = spriteLayer.transform.DOLocalRotate(rockSpinRotation, .2f).SetLoops(-1, LoopType.Restart);
        StartCoroutine(WaitTimer());
        audioSource.pitch = UnityEngine.Random.Range(2.5f, 4f);
        audioSource.Play();
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
            BreakRock();
        }
    }

    private void BreakRock()
    {
        audioSource.Stop();
        spinRock.Kill();
        Destroy(rockPrefab, .5f);
    }

    private IEnumerator WaitTimer()
    {
        yield return new WaitForSeconds(10);
        BreakRock();
    }
}
