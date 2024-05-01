using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MongoDB.Driver;
using MongoDB.Bson;

public class CoinManager : MonoBehaviour
{
    public TextMeshProUGUI Cointext;
    public float Totalcoins;

    // Key for saving/loading the coin amount
    private const string CoinSaveKey = "TotalCoins";
    private IMongoCollection<BsonDocument> coinsCollection;

    // Called when the script instance is being loaded
    private void Awake()
    {
        // Connect to MongoDB
        MongoClient client = new MongoClient("mongodb+srv://Equipo4Neoris:Neoris2024@neorisdb.pndj61t.mongodb.net/?retryWrites=true&w=majority&appName=NeorisDB");
        IMongoDatabase database = client.GetDatabase("test");
        coinsCollection = database.GetCollection<BsonDocument>("coins");

        LoadCoins();
    }

    // Method to add coins
    public void AddCoins()
    {
        Totalcoins += 5;
        UpdateCoinText();
        SaveCoins();
    }

    public void DecreaseCoins(float amount)
    {
        Totalcoins -= amount;
        UpdateCoinText();
        SaveCoins();
    }

    private void UpdateCoinText()
    {
        Cointext.text = Totalcoins.ToString("0");
    }

    private void SaveCoins()
    {
        // Update the coins in the MongoDB collection
        var filter = Builders<BsonDocument>.Filter.Empty;
        var update = Builders<BsonDocument>.Update.Set("coins", Totalcoins);
        coinsCollection.UpdateOne(filter, update, new UpdateOptions { IsUpsert = true });
    }

    // Method to load the saved coin amount
    private void LoadCoins()
    {
        // Retrieve the coins from the MongoDB collection
        var filter = Builders<BsonDocument>.Filter.Empty;
        var document = coinsCollection.Find(filter).FirstOrDefault();

        if (document != null && document.TryGetValue("coins", out var coins))
        {
            Totalcoins = (float)coins.AsDouble;
            UpdateCoinText();
        }
        else
        {
            Totalcoins = 0;
            SaveCoins();
        }
    }
}
