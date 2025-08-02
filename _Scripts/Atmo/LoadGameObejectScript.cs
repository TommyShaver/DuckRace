using UnityEditor.PackageManager;
using UnityEngine;

public class LoadGameObejectScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Ispawn hit = other.GetComponent<Ispawn>();
        if (hit != null)
        {
            hit.OnSpawn();
        }
    }
}
