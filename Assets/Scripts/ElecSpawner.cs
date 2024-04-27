using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElecSpawner : MonoBehaviour
{
    [SerializeField] private GameObject elecPrefab;

    private void Start()
    {
        // Get the count of "Elecslime" cards from the GachaManager and spawn elecslimes accordingly
        int elecCount = GachaManager.GetCardCount("Elecslime");
        SpawnElecSlimes(elecCount);
    }

    private void SpawnElecSlimes(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(elecPrefab, transform.position, Quaternion.identity);
        }
    }
}
