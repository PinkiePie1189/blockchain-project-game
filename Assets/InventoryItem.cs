using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    
    public string itemName;
    public int itemUpgrade;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateItem()
    {
        GetComponentInChildren<TMP_Text>().text = $"{itemName} + {itemUpgrade}";
        GetComponent<Image>().sprite = Resources.Load<Sprite>($"{itemName.ToLower()}");
        
    }
}
