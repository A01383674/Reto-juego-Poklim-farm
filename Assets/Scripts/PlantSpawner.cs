using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSpawner : MonoBehaviour
{
    [SerializeField] private GameObject plantPrefab;

    private void Start()
    {
        // Get the count of "Plantslime" cards from the GachaManager and spawn plants accordingly
        int plantCount = GachaManager.GetCardCount("Plantslime");
        SpawnPlants(plantCount);
    }

    private void SpawnPlants(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(plantPrefab, transform.position, Quaternion.identity);
        }
    }
}
