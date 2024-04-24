using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import TMP namespace

public class CoinManager : MonoBehaviour
{
    public TextMeshProUGUI Cointext; // Use TextMeshProUGUI for text
    public float Totalcoins;

    // Key for saving/loading the coin amount
    private const string CoinSaveKey = "TotalCoins";

    // Called when the script instance is being loaded
    private void Awake()
    {
        
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
        PlayerPrefs.SetFloat(CoinSaveKey, Totalcoins);
        PlayerPrefs.Save(); 
    }

    // Method to load the saved coin amount
    private void LoadCoins()
    {
        if (PlayerPrefs.HasKey(CoinSaveKey))
        {
            Totalcoins = PlayerPrefs.GetFloat(CoinSaveKey);
            UpdateCoinText();
        }
        else
        {
            Totalcoins = 0; 
            SaveCoins();
        }
    }
}
