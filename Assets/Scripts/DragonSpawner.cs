using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonSpawner : MonoBehaviour
{
    [SerializeField] private GameObject dragonPrefab;

    private void Start()
    {
        // Get the count of "Dragonslime" cards from the GachaManager and spawn dragons accordingly
        int dragonCount = GachaManager.GetCardCount("Dragonslime");
        SpawnDragons(dragonCount);
    }

    private void SpawnDragons(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(dragonPrefab, transform.position, Quaternion.identity);
        }
    }
}
