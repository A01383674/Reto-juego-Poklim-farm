using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject waterPrefab;

    private void Start()
    {
        // Get the count of "Waterslime" cards from the GachaManager and spawn water slimes accordingly
        int waterCount = GachaManager.GetCardCount("Waterslime");
        SpawnWaterSlimes(waterCount);
    }

    private void SpawnWaterSlimes(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(waterPrefab, transform.position, Quaternion.identity);
        }
    }
}
