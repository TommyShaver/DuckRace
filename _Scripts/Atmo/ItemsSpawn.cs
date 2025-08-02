using System.Collections;
using UnityEngine;

public class ItemsSpawn : MonoBehaviour
{
    [SerializeField] private GameObject itemBlockPrefab;
    [SerializeField] private GameObject holdItemsGameObject;
    private float spawnPosX;
    private float[] placementArray = { 1, 0, -1, -2, -3, -4, -5};
    private SpriteRenderer spriteRenderer;
    private bool canSpawnItemBlock = true;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
      
    }

    private void Osable()
    {
     
    }

    private void Start()
    {
        Color currentColor = spriteRenderer.color;
        currentColor.a = 0;
        spriteRenderer.color = currentColor;
        spawnPosX = transform.position.x;
        if (canSpawnItemBlock)
        {
            SpawnItemBlockOnLoad();
        }
    }

    //Incoming Logic ............................
    private void LetItemBlockSpawn(bool canSpawn)
    {
        canSpawnItemBlock = canSpawn;
    }



    //Spawn Item Block Logic ....................
    private void SpawnItemBlockOnLoad()
    {
        for (int i = 0; i < 7; i++)
        {
            Instantiate(itemBlockPrefab, new Vector3(spawnPosX, placementArray[i], 0), itemBlockPrefab.transform.rotation, holdItemsGameObject.transform);
        }
    }
}
