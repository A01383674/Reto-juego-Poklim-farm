using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;

public class GachaManager : MonoBehaviour
{
    [SerializeField] private GachaRate[] gacha;
    [SerializeField] private Transform parent, pos;
    [SerializeField] private GameObject characterCardGO;
    CoinManager coinManager; // Reference to CoinManager script
    private IMongoCollection<BsonDocument> cardCountsCollection;

    private static Dictionary<string, int> cardCounts = new Dictionary<string, int>();

    private void Start()
    {
        coinManager = FindObjectOfType<CoinManager>();

        // Load existing card counts from PlayerPrefs
        LoadCardCountsFromPlayerPrefs();

        // Connect to the MongoDB database
        MongoClient client = new MongoClient("mongodb+srv://Equipo4Neoris:Neoris2024@neorisdb.pndj61t.mongodb.net/?retryWrites=true&w=majority&appName=NeorisDB");
        IMongoDatabase database = client.GetDatabase("test");

        // Get the card counts collection
        cardCountsCollection = database.GetCollection<BsonDocument>("card_counts");
    }

    private void LoadCardCountsFromPlayerPrefs()
    {
        // Load existing card counts from PlayerPrefs
        foreach (var kvp in cardCounts)
        {
            string cardName = kvp.Key;
            int count = PlayerPrefs.GetInt(cardName + "_Count", 0);
            cardCounts[cardName] = count;
        }
    }

    public static void Gacha()
    {
        GachaManager instance = FindObjectOfType<GachaManager>();
        if (instance != null && instance.coinManager != null && instance.coinManager.Totalcoins >= 5)
        {
            bool cardCreated = false;
            while (!cardCreated && instance.coinManager.Totalcoins >= 5)
            {
                cardCreated = instance.CreateCharacterCard(); // Attempt to create a character card
                if (cardCreated)
                {
                    // Decrease 5 coins from the user's total coins
                    instance.coinManager.DecreaseCoins(5);
                }
            }
            if (!cardCreated)
            {
                Debug.Log("Failed to obtain a card after multiple attempts.");
            }
        }
        else
        {
            Debug.Log("Not enough coins to perform the gacha.");
        }
    }


    bool CreateCharacterCard()
    {
        GameObject characterCard = Instantiate(characterCardGO, pos.position, Quaternion.identity) as GameObject;
        characterCard.transform.SetParent(parent);
        characterCard.transform.localScale = new Vector3(1, 1, 1);
        Cards card = characterCard.GetComponent<Cards>();

        int rnd = UnityEngine.Random.Range(0, 101); // Generate random number between 0 and 100 (inclusive)
        int cumulativeRate = 0;

        // Iterate over gacha rates to find the appropriate rarity based on the random number
        for (int i = 0; i < gacha.Length; i++)
        {
            cumulativeRate += gacha[i].rate;
            if (rnd <= cumulativeRate)
            {
                card.card = Reward(gacha[i].rarity);
                // Save the obtained card name in PlayerPrefs
                SaveCardData(card.card.name);
                UpdateCardCount(card.card.name); // Update card count
                return true; // Card created successfully
            }
        }

        Destroy(characterCard); // Destroy the card if not created successfully
        return false; // Card not created

    }

    cardinfo Reward(string rarity)
    {
        GachaRate gr = System.Array.Find(gacha, rt => rt.rarity == rarity);
        cardinfo[] reward = gr.reward;

        int rnd = UnityEngine.Random.Range(0, reward.Length);
        return reward[rnd];
    }

    void SaveCardData(string cardName)
    {
        // Save the card name in PlayerPrefs
        PlayerPrefs.SetString("ObtainedCardName", cardName);
        PlayerPrefs.Save();
    }

    void UpdateCardCount(string cardName)
    {
        // Check if the card is already in the dictionary
        if (cardCounts.ContainsKey(cardName))
        {
            // If yes, increment the count
            cardCounts[cardName]++;
        }
        else
        {
            // If not, add it to the dictionary with count 1
            cardCounts.Add(cardName, 1);
        }

        // Update the count in PlayerPrefs
        PlayerPrefs.SetInt(cardName + "_Count", cardCounts[cardName]);
        PlayerPrefs.Save();

        // Update the count in the MongoDB database
        var filter = Builders<BsonDocument>.Filter.Eq("cardName", cardName);
        var update = Builders<BsonDocument>.Update.Set("count", cardCounts[cardName]);
        cardCountsCollection.UpdateOne(filter, update, new UpdateOptions { IsUpsert = true });

        Debug.Log("Card Count: " + cardName + " = " + cardCounts[cardName]);
    }

    // Method to get the count of a specific card
    public static int GetCardCount(string cardName)
    {
        if (cardCounts.ContainsKey(cardName))
        {
            return cardCounts[cardName];
        }
        else
        {
            return 0;
        }
    }
}
