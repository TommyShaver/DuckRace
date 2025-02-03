using System.Collections.Generic;
using UnityEngine;

public class PlayField : MonoBehaviour
{
    public GameObject spawnWaterTrap;
    private int spawnCount = 10;
    private List<GameObject> spawnObjectList = new List<GameObject>();

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
                if(obj != null && newBoundsTry.Intersects(sripteBoudns.bounds))
                {
                    overlapingBounds = true;
                    i--; //try again.
                    break;
                }
            }
            if (!overlapingBounds)
            {
                GameObject spawnedObject = Instantiate(spawnWaterTrap, spawnHere, Quaternion.identity);
                spawnObjectList.Add(spawnedObject);
            }
        }
    }

    private Vector2 GetRandomPostion()
    {
        Bounds bounds = this.GetComponent<SpriteRenderer>().bounds;
        Vector2 randomPosition = new Vector2(
           Random.Range(bounds.min.x + 2, bounds.max.x - 2),
           Random.Range(bounds.min.y + 1, bounds.max.y - 1));
        return randomPosition;
    }
}
