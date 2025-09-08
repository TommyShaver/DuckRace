using UnityEngine;

public class TestAllDucks : MonoBehaviour
{
    int i;
    public void AddDucks()
    {
        i++;
        SpawnManager.Instance.IncomingData(i.ToString());
    }
}
