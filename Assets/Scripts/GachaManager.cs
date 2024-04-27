using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaManager : MonoBehaviour
{
    [SerializeField] private GachaRate[] gacha;
    [SerializeField] private Transform parent, pos;
    [SerializeField] private GameObject characterCardGO;
    CoinManager coinManager; // Reference to CoinManager script

    private static Dictionary<string, int> cardCounts = new Dictionary<string, int>();

    private void Start()
    {
        coinManager = FindObjectOfType<CoinManager>();
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
