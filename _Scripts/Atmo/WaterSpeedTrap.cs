using UnityEngine;
using DG.Tweening;

public class WaterSpeedTrap : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animationPath;
    [SerializeField] Color[] startingColors;
    private Tween waveScaling;
    [SerializeField]private bool speedUp;
    private bool iHaveNumber;
    private int idName;


    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animationPath = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        WaterTrapSpawner.OnCreateWaterTrapPrefab += OnSpawn;
        WaterTrapSpawner.OnSendWaterTrapTransform += OnUpdateTranform;
        WaterTrapSpawner.OnWaterTrapAnimationUp += OnAnimationUp;
        WaterTrapSpawner.OnDeSpawnWaterTrap += OnDespawn;
    }

    private void OnDisable()
    {
        WaterTrapSpawner.OnCreateWaterTrapPrefab -= OnSpawn;
        WaterTrapSpawner.OnSendWaterTrapTransform -= OnUpdateTranform;
        WaterTrapSpawner.OnWaterTrapAnimationUp -= OnAnimationUp;
        WaterTrapSpawner.OnDeSpawnWaterTrap -= OnDespawn;
    }

    //? Collision logic ------------------------------------------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DuckInterractionInterface hit = collision.GetComponent<DuckInterractionInterface>();
        if (hit != null)
        {
            if (!speedUp)
            {
                hit.DuckInterraction("SlownDown", true);
            }
            else
            {
                hit.DuckInterraction("SpeedUp", true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        DuckInterractionInterface hit = collision.GetComponent<DuckInterractionInterface>();
        if (hit != null)
        {
            if (!speedUp)
            {
                hit.DuckInterraction("SlownDown", false);
            }
            else
            {
                hit.DuckInterraction("SpeedUp", false);
            }
        }
    }

    //? In bound logic ..............................
    private void OnSpawn(int sentNumber)
    {
        if (!iHaveNumber)
        {
            idName = sentNumber;
            iHaveNumber = true;
        }
    }
    private void OnUpdateTranform(Vector3 spawnPos, int sentNumber)
    {
        if (idName != sentNumber)
        {
            return;
        }
        speedUp = false;
        transform.position = spawnPos;
        int yPos = Mathf.RoundToInt(transform.position.y);
        SetLayer(yPos);
        speedUp = UnityEngine.Random.Range(0, 2) == 0;
        WaterDirection();
        spriteRenderer.color = startingColors[Random.Range(0, startingColors.Length)];
        animationPath.speed = Random.Range(0.2f, 0.5f);
    }

    private void OnAnimationUp()
    {
        transform.DOScaleY(1.0138f, .5f).SetEase(Ease.OutExpo);
    }

    private void OnDespawn()
    {
        speedUp = false;
        waveScaling.Kill();
        transform.DOScaleY(0, .5f).SetEase(Ease.OutExpo);
    }

    //? Helper funcitono ............................
    private void WaterDirection()
    {
        if (!speedUp)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }

    }

    private int RandomNumber()
    {
        int randomNumber = Random.Range(1, 5);
        return randomNumber;
    }

    private int SetLayer(int pos)
    {
        int baseSortingOrder = 50;
        int offset = -pos;
        spriteRenderer.sortingOrder = baseSortingOrder + offset;
        return baseSortingOrder;
    }
}
