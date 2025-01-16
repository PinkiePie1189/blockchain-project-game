using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        init();
    }

    public void init()
    {
        var em = GameObject.FindGameObjectWithTag("Equipment").GetComponent<EquipmentManager>();

        string[] names = {
            "Helmet",
            "Chestplate",
            "Gloves",
            "Boots",
            "Neck",
            "Ring",
            "Sword"
        };


        for (int i = 0; i < names.Length; i++)
        {
            if (em.equipmentAddresses[names[i].ToLower()] != null)
            {
                var obj = transform.GetChild(i);
                obj.GetComponent<Image>().sprite = Resources.Load<Sprite>(names[i].ToLower());
                obj.GetComponentInChildren<TMP_Text>().text = $"{names[i]} + {em.equipmentUpgrade[names[i].ToLower()]}";
            }
        }
        
            
        var atk =  Math.Ceiling(5 + (em.equipmentUpgrade["sword"] * 0.1 * 5.0));
        var hp = Math.Ceiling(100 + (em.equipmentUpgrade["helmet"] * 0.1 * 100.0));
        var armor = Math.Ceiling(2 + (em.equipmentUpgrade["chestplate"] * 0.1 * 2.0));
        var acc = Math.Ceiling(em.equipmentUpgrade["gloves"] * 0.1 * 10.0);
        var evasion = Math.Ceiling(em.equipmentUpgrade["boots"] * 0.1 * 10.0);
        var crit = Math.Ceiling(em.equipmentUpgrade["neck"] * 0.1 * 5.0)
                   + Math.Ceiling(em.equipmentUpgrade["ring"] * 0.1 * 5.0);
        
        
        var statsText = $"ATK: {atk}\nHP:{hp}\nArmor:{armor}\nAcc:{acc}\nEvasion:{evasion}\nCrit: {crit}";
        
        transform.GetChild(7).gameObject.GetComponent<TextMeshProUGUI>().text = statsText;
    }
}
