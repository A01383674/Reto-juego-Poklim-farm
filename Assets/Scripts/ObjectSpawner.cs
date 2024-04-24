using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [System.Serializable]
    public class RewardMapping
    {
        public string cardName;
        public GameObject prefab;
    }

    [SerializeField] private List<RewardMapping> rewardMappings = new List<RewardMapping>();
    private List<GameObject> spawnedObjects = new List<GameObject>(); // Keep track of spawned objects

    void Start()
    {
        // Check if the player obtained a card
        if (PlayerPrefs.HasKey("ObtainedCardName"))
        {
            string obtainedCardName = PlayerPrefs.GetString("ObtainedCardName");

            // Find all corresponding reward mappings
            List<RewardMapping> mappings = rewardMappings.FindAll(x => x.cardName == obtainedCardName);

            if (mappings != null)
            {
                // Spawn the corresponding prefabs for each mapping
                foreach (var mapping in mappings)
                {
                    Vector3 spawnPosition = transform.position + Random.insideUnitSphere * 2f; // Randomize position within a radius
                    GameObject spawnedObject = Instantiate(mapping.prefab, spawnPosition, Quaternion.identity);
                    spawnedObjects.Add(spawnedObject); // Add the spawned object to the list
                }
            }

            // Clear the saved card data AFTER spawning all rewards
            PlayerPrefs.DeleteKey("ObtainedCardName");
            PlayerPrefs.Save();
        }
    }

    // Call this method to manually spawn a reward
    public void SpawnReward(string cardName)
    {
        // Find all corresponding reward mappings
        List<RewardMapping> mappings = rewardMappings.FindAll(x => x.cardName == cardName);

        if (mappings != null)
        {
            // Spawn the corresponding prefabs for each mapping
            foreach (var mapping in mappings)
            {
                Vector3 spawnPosition = transform.position + Random.insideUnitSphere * 2f; // Randomize position within a radius
                GameObject spawnedObject = Instantiate(mapping.prefab, spawnPosition, Quaternion.identity);
                spawnedObjects.Add(spawnedObject); // Add the spawned object to the list
            }
        }
    }
}
