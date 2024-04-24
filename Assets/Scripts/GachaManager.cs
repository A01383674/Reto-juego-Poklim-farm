using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GachaManager : MonoBehaviour
{
    [SerializeField] private GachaRate[] gacha;
    [SerializeField] private Transform parent, pos;
    [SerializeField] private GameObject characterCardGO;
    CoinManager coinManager; // Reference to CoinManager script

    // Add a reference to CoinManager script
    private void Start()
    {
        coinManager = FindObjectOfType<CoinManager>();
    }

    public void Gacha()
    {
        // Check if the user has more than 5 coins
        if (coinManager != null && coinManager.Totalcoins >= 5)
        {
            // Decrease 5 coins from the user's total coins
            coinManager.DecreaseCoins(5);

            // Create character card
            CreateCharacterCard();
        }
        else
        {
            Debug.Log("Not enough coins to perform the gacha.");
        }
    }

    void CreateCharacterCard()
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
                return;
            }
        }
    }

    cardinfo Reward(string rarity)
    {
        GachaRate gr = Array.Find(gacha, rt => rt.rarity == rarity);
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
}
