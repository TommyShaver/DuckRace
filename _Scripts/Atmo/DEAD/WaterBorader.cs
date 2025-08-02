using UnityEngine;

public class WaterBorader : MonoBehaviour
{
    public GameObject waterPrefab;
    public GameObject grassPrefab;
    public GameObject housingGameObject;

    public bool veritcalSpawn;
    public bool isWaterPrefab;

    public int spawnCount;
    public float spawnLenght;

    private Vector3 updatedPostion;
    private Vector3 waterSpawnPostion; // Water Block
    public Vector3 grassSpawnPostion;


    private void Start()
    {
        waterSpawnPostion = waterPrefab.transform.position;
        if(!veritcalSpawn)
        {
            SpawnWaterBorader();
        }
        else
        {
            SpawnWAterBoraderY();
        }
    }

    private void SpawnWaterBorader()
    {
        updatedPostion = WhichTransform();
        for (int i = 0; i < spawnCount; i++)
        {
            if(!isWaterPrefab)
            {
                FlipGrassBlockSprite();
            }
            updatedPostion.x += spawnLenght;
            Instantiate(WhichPrefab(), updatedPostion, WhichPrefab().transform.rotation, housingGameObject.transform);
        }
    }
    
    private void SpawnWAterBoraderY()
    {
        updatedPostion = WhichTransform();
        for (int i = 0; i < spawnCount; i++)
        {
            if (!isWaterPrefab)
            {
                FlipGrassBlockSprite();
            }
            updatedPostion.y -= spawnLenght;
            Instantiate(WhichPrefab(), updatedPostion, WhichPrefab().transform.rotation, housingGameObject.transform);
        }
    }

    private GameObject WhichPrefab()
    {
        GameObject prefabChoice;
        if (isWaterPrefab)
        {
            prefabChoice = waterPrefab;
        }
        else
        {
            prefabChoice = grassPrefab;
        }
        return prefabChoice;
    }

    private Vector3 WhichTransform()
    {
        Vector3 spawnPostionOfPrefab = new Vector3();
        if (isWaterPrefab)
        {
            spawnPostionOfPrefab = waterSpawnPostion;
        }
        else
        {
            spawnPostionOfPrefab = grassSpawnPostion;
        }
        return spawnPostionOfPrefab;
    }

    private void FlipGrassBlockSprite()
    {
        grassPrefab.GetComponent<SpriteRenderer>().flipX = RandoBool();
        grassPrefab.GetComponent<SpriteRenderer>().flipY = RandoBool(); 
    }
    private bool RandoBool()
    {
        int number = Random.Range(0, 10);
        if (number <= 5)
            return false;
        else
            return true;   
        
    }
}
