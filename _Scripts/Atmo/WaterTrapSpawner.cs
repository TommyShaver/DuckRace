using System;
using System.Collections.Generic;
using UnityEngine;

public class WaterTrapSpawner : MonoBehaviour
{
    public static WaterTrapSpawner Instance { get; private set; }
    public static event Action<int> OnCreateWaterTrapPrefab;
    public static event Action<Vector3, int> OnSendWaterTrapTransform;
    public static event Action OnWaterTrapAnimationUp;
    public static event Action OnDeSpawnWaterTrap;
    public GameObject waterTrapPrefab;
    public GameObject storedGameObjectPrefabs;

    [SerializeField] private int spawnCount = 100;

    private HashSet<Vector2> storeSpawn = new HashSet<Vector2>();

    private void Awake()
    {
         if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Vector3 defaultSpawnPos = new(500, 500, 0);

        for (int i = 0; i <= spawnCount; i++)
        {
            Instantiate(waterTrapPrefab, defaultSpawnPos, waterTrapPrefab.transform.localRotation, storedGameObjectPrefabs.transform);
            OnCreateWaterTrapPrefab?.Invoke(i);
        }
    }

    //Core logic ..........................
    public void WaterTrapsTransformUpdate()
    {
        for (int i = 0; i <= spawnCount; i++)
        {
            Vector3 spawnPostion = GetPos();
            OnSendWaterTrapTransform?.Invoke(spawnPostion, i);
        }
    }

    public void WaterTrapsAnimationUp()
    {
        OnWaterTrapAnimationUp?.Invoke();
    }

    public void WaterTrapsDespawn()
    {
        OnDeSpawnWaterTrap?.Invoke();
    }

    //HelperFunctoin .....................................
    private Vector2 GetPos()
    {
        int[] notTheseNumber = { 50, 90, 125, 160, 200 };
        int yPos = UnityEngine.Random.Range(-5, 1);
        Vector2 newPostion;
        while (true)
        {
            int findX = UnityEngine.Random.Range(6, 255);
            if (findX % 2 == 0)
            {
                findX++;
            }
            if (System.Array.Exists(notTheseNumber, x => x == findX))
                continue;
            newPostion = new Vector2(findX, yPos);
            bool isUnique = true;
            foreach (Vector2 pos in storeSpawn)
            {
                if (pos == newPostion)
                {
                    isUnique = false;
                    break;
                }
            }
            if (isUnique)
            {
                storeSpawn.Add(newPostion);
                return newPostion;
            }
        }
    }
}
