using System.Collections.Generic;
using Solana.Unity.Wallet;
using UnityEngine;
public class EquipmentManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Dictionary<string, string> equipmentAddresses =  
        new Dictionary<string, string>(){
            {"helmet", null},  	 	
            {"chestplate", null},
            {"neck", null},
            {"ring", null},
            {"boots", null},
            {"sword", null},
            {"gloves", null}
        };

    public Dictionary<string, int> equipmentUpgrade = new Dictionary<string, int>()
    {
        { "helmet", 0 },
        { "chestplate", 0 },
        { "neck", 0 },
        { "ring", 0 },
        { "boots", 0 },
        { "sword", 0 },
        { "gloves", 0 }
    };
}
