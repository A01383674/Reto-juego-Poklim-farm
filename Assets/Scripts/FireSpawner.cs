using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpawner : MonoBehaviour
{
    [SerializeField] private GameObject firePrefab;

    private void Start()
    {
        // Get the count of "Fireslime" cards from the GachaManager and spawn fireslimes accordingly
        int fireCount = GachaManager.GetCardCount("Fireslime");
        SpawnFireSlimes(fireCount);
    }

    private void SpawnFireSlimes(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(firePrefab, transform.position, Quaternion.identity);
        }
    }
}
