using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cloudPrefab;

    private void Start()
    {
        // Get the count of "Cloudslime" cards from the GachaManager and spawn cloud slimes accordingly
        int cloudCount = GachaManager.GetCardCount("Cloudslime");
        SpawnCloudSlimes(cloudCount);
    }

    private void SpawnCloudSlimes(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(cloudPrefab, transform.position, Quaternion.identity);
        }
    }
}
