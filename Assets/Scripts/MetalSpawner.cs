using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalSpawner : MonoBehaviour
{
    [SerializeField] private GameObject metalPrefab;

    private void Start()
    {
        // Get the count of "MetalSlime" cards from the GachaManager and spawn metal slimes accordingly
        int metalCount = GachaManager.GetCardCount("MetalSlime");
        SpawnMetalSlimes(metalCount);
    }

    private void SpawnMetalSlimes(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(metalPrefab, transform.position, Quaternion.identity);
        }
    }
}
