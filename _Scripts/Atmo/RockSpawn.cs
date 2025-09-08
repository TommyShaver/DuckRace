using Unity.Mathematics;
using UnityEngine;

public class RockSpawn : MonoBehaviour
{

    [SerializeField] private GameObject boulderPrefab;
    [SerializeField] private GameObject holdItemsGameObject;
    private float spawnPosX;
    private float[] placementArray = { 1, 0, -1, -2, -3 , -4, -5};
    private SpriteRenderer spriteRenderer;
    private bool canSpawnRocks = true;
    private float checkLastPos;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        UIButtonScript.OnUIButtonSwitch += LetRocksSpawn;
        GameManager.OnRockPlacementUpdate += SpawnRock;
    }

    private void OnDisable()
    {
        UIButtonScript.OnUIButtonSwitch -= LetRocksSpawn;
        GameManager.OnRockPlacementUpdate -= SpawnRock;
    }

    void Start()
    {
        canSpawnRocks = SaveDataManager.instance.rockSpawn;
        Color currentColor = spriteRenderer.color;
        currentColor.a = 0;
        spriteRenderer.color = currentColor;
        spawnPosX = transform.position.x;
    }

    //Incoming logic ........................  
    private void LetRocksSpawn(bool canSpawn, string command)
    {
        canSpawnRocks = canSpawn;
        
        if (canSpawn && command == "rock")
            SpawnRock();
    }

    //Spanw Rock logic .......................
    private void SpawnRock()
    {
        if (canSpawnRocks)
        {
            for (int i = 0; i < 2; i++)
            {
                Instantiate(boulderPrefab, RandomSpawnPos(), boulderPrefab.transform.rotation, holdItemsGameObject.transform);
            }
        }
    }

    private Vector3 RandomSpawnPos()
    {
        Vector3 updatedPos;

        while (true)
        {
            float newPosY = placementArray[UnityEngine.Random.Range(0, placementArray.Length)];
            if (newPosY != checkLastPos)
            {
                updatedPos = new(spawnPosX, newPosY, 0);
                checkLastPos = newPosY;
                break;
            }
        }
       
        return updatedPos;
    }
}
