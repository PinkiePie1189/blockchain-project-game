using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Solana.Unity.Wallet;

public class InventoryItem : MonoBehaviour
{
    
    public string itemName;
    public int itemUpgrade;

    public PublicKey nftAddress;
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

    public void Equip()
    {
        Debug.Log("Equip called");
        
        GameObject equipmentManager = GameObject.FindGameObjectWithTag("Equipment");

        equipmentManager.GetComponent<EquipmentManager>().equipmentAddresses[itemName.ToLower()] = nftAddress;
        equipmentManager.GetComponent<EquipmentManager>().equipmentUpgrade[itemName.ToLower()] = itemUpgrade;
        
    }
}
