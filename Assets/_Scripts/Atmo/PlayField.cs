using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;

public class PlayField : MonoBehaviour
{
    public static PlayField instance;
    public GameObject spawnWaterTrap;
    [SerializeField] private int spawnCount = 10;
    private List<GameObject> spawnObjectList = new List<GameObject>();


    public static event Action OnDespawn;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        SpawnHeadWaterTrap();
    }
    private void SpawnHeadWaterTrap()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 spawnHere = GetRandomPostion();

            GameObject tempGameObejct = Instantiate(spawnWaterTrap, spawnHere, Quaternion.identity);
            Bounds newBoundsTry = tempGameObejct.GetComponent<SpriteRenderer>().bounds;
            bool overlapingBounds = false;
            Destroy(tempGameObejct);
            foreach (GameObject obj in spawnObjectList)
            {
                SpriteRenderer sripteBoudns = obj.GetComponent<SpriteRenderer>();
                if (obj != null && newBoundsTry.Intersects(sripteBoudns.bounds))
                {
                    overlapingBounds = true;
                    i--; //try again.
                    break;
                }
            }
            if (!overlapingBounds)
            {
                GameObject spawnedObject = Instantiate(spawnWaterTrap, spawnHere, Quaternion.identity); // adding a housing made unity crash spawn on the inspector as they please jerks.
                spawnObjectList.Add(spawnedObject);
            }
        }
    }

    private Vector2 GetRandomPostion()
    {
        Bounds bounds = this.GetComponent<SpriteRenderer>().bounds;
        Vector2 randomPosition = new Vector2(
          UnityEngine.Random.Range(bounds.min.x + 2, bounds.max.x - 2),
          UnityEngine.Random.Range(bounds.min.y + 1, bounds.max.y - 1));
        return randomPosition;
    }

    public void DespwanTrapsObject()
    {
        spawnObjectList.Clear();
        OnDespawn?.Invoke();
        StartCoroutine(WaitTime());
    }

    private IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(1);
        SpawnHeadWaterTrap();
    }
}
