using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Cards : MonoBehaviour
{
    public cardinfo card;
    [SerializeField] private Image img;
    [SerializeField] private TextMeshProUGUI name;
    // Start is called before the first frame update
    void Start()
    {
        if(card != null)
        {
            img.sprite = card.image;
            name.text = card.name;
        }


    }

    // Update is called once per frame
    void Update()
    {
        Start();
    }
}
