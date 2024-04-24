using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance

    public CoinManager coinManager; // Reference to CoinManager script
    private Dictionary<string, int> playerSlimes = new Dictionary<string, int>(); // Dictionary to store player's slimes

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        coinManager = FindObjectOfType<CoinManager>();
    }

    public void AddSlime(string slimeName)
    {
        if (playerSlimes.ContainsKey(slimeName))
        {
            playerSlimes[slimeName]++;
        }
        else
        {
            playerSlimes.Add(slimeName, 1);
        }
    }

    public void SpawnSlimes()
    {
        foreach (var slimeEntry in playerSlimes)
        {
            string slimeName = slimeEntry.Key;
            int quantity = slimeEntry.Value;

            GameObject slimePrefab = FindSlimePrefab(slimeName);

            if (slimePrefab != null)
            {
                for (int i = 0; i < quantity; i++)
                {
                    // Modify the spawn position as needed
                    Instantiate(slimePrefab, GetRandomSpawnPosition(), Quaternion.identity);
                }
            }
        }
    }

    private GameObject FindSlimePrefab(string slimeName)
    {
        // Modify this method to find the slime prefab from your resources
        // or from a specific directory
        return Resources.Load<GameObject>("SlimePrefabs/" + slimeName);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        return new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
    }
}
