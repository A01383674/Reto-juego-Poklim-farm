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

    private static Dictionary<string, int> cardCounts = new Dictionary<string, int>();

    private MongoClient mongoClient;
    private IMongoDatabase database;
    private IMongoCollection<BsonDocument> usersCollection;

    private void Start()
    {
        coinManager = FindObjectOfType<CoinManager>();

        // Connect to MongoDB
        mongoClient = new MongoClient("mongodb+srv://Equipo4Neoris:Neoris2024@neorisdb.pndj61t.mongodb.net/?retryWrites=true&w=majority&appName=NeorisDB");
        database = mongoClient.GetDatabase("test");
        usersCollection = database.GetCollection<BsonDocument>("users");

        // Get the user's slime count from the database
        int slimeCount = GetSlimeCountFromDatabase();

        // Set the count of the cards to the slime count
        SetCardCount("Slime", slimeCount);

        // Load existing card counts from PlayerPrefs
        LoadCardCountsFromPlayerPrefs();
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

    private int GetSlimeCountFromDatabase()
    {
        // Query the database to get the user's slime count
        var filter = Builders<BsonDocument>.Filter.Eq("username", "your_username");
        var projection = Builders<BsonDocument>.Projection.Include("slimeCount");
        var result = usersCollection.Find(filter).Project(projection).FirstOrDefault();

        if (result != null && result.Contains("slimeCount"))
        {
            return result["slimeCount"].AsInt32;
        }
        else
        {
            return 0;
        }
    }

    private void SetCardCount(string cardName, int count)
    {
        // Set the count of the specified card
        cardCounts[cardName] = count;
    }

    public static void Gacha()
    {
        GachaManager instance = FindObjectOfType<GachaManager>();
        if (instance != null && instance.coinManager != null && instance.coinManager.Totalcoins >= 5)
        {
            bool cardCreated = instance.CreateCharacterCard(); // Attempt to create a character card

            // If no card is created, reroll without decreasing coins
            if (!cardCreated)
            {
                Gacha(); // Reroll without decreasing coins
            }
            else
            {
                // Decrease 5 coins from the user's total coins
                instance.coinManager.DecreaseCoins(5);
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

        int rnd = UnityEngine.Random.Range(1, 101);
        for (int i = 0; i < gacha.Length; i++)
        {
            if (rnd <= gacha[i].rate)
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
