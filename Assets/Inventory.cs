using System;
using System.Collections.Generic;
using Solana.Unity.Wallet;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public struct Item
{
    public Sprite sprite;
    public PublicKey nftAddress;
}


public class Inventory : MonoBehaviour
{
    
    // Slots
    public GameObject swordSlot;
    public GameObject helmetSlot;
    public GameObject chestplateSlot;
    public GameObject bootsSlot;
    public GameObject neckSlot;
    public GameObject ringSlot;
    public GameObject glovesSlot;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("aici");
        equip(PublicKey.DefaultPublicKey, "Sword");
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void equip(PublicKey nftAddress, string slotName)
    {

        var sprite = Resources.Load<Sprite>(slotName.ToLower());
        switch (slotName) {
            case "Sword":
            {
                swordSlot.GetComponent<ItemSocket>().Equip(sprite, nftAddress);
                break;
            }

            case "Helmet": {
                helmetSlot.GetComponent<Image>().sprite = sprite;
                break;
            }

            case "Chestplate": {
                chestplateSlot.GetComponent<Image>().sprite = sprite;
                break;
            }

            case "Boots": {
                bootsSlot.GetComponent<Image>().sprite = sprite;
                break;
            }

            case "Neck": {
                neckSlot.GetComponent<Image>().sprite = sprite;
                break;
            }

            case "Ring":
            {
                ringSlot.GetComponent<Image>().sprite = sprite;
                break;
            }

            case "Gloves":
            {
                glovesSlot.GetComponent<Image>().sprite = sprite;
                break;
            }
                
        }
    }
}
