using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject slimePrefab;

    private void Start()
    {
        // Get the count of "Slime" cards from the GachaManager and spawn slimes accordingly
        int slimeCount = GachaManager.GetCardCount("Slime");
        SpawnSlimes(slimeCount);
    }

    private void SpawnSlimes(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(slimePrefab, transform.position, Quaternion.identity);
        }
    }
}