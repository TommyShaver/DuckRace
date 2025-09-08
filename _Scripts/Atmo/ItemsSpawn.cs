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
        UIButtonScript.OnUIButtonSwitch += LetItemBlockSpawn;
    }

    private void OnDisable()
    {
        UIButtonScript.OnUIButtonSwitch -= LetItemBlockSpawn;
    }

    private void Start()
    {
        canSpawnItemBlock = SaveDataManager.instance.itemsSpawn;
        Color currentColor = spriteRenderer.color;
        currentColor.a = 0;
        spriteRenderer.color = currentColor;
        spawnPosX = transform.position.x;
        SpawnItemBlockOnLoad();

    }

    //Incoming Logic ............................
    private void LetItemBlockSpawn(bool canSpawn, string command)
    {
        canSpawnItemBlock = canSpawn;
        if (canSpawn && command == "item")
            SpawnItemBlockOnLoad();
    }



    //Spawn Item Block Logic ....................
    private void SpawnItemBlockOnLoad()
    {
        if (canSpawnItemBlock)
        {
            for (int i = 0; i < 7; i++)
            {
                Instantiate(itemBlockPrefab, new Vector3(spawnPosX, placementArray[i], 0), itemBlockPrefab.transform.rotation, holdItemsGameObject.transform);
            }
        }
    }
}
