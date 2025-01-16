using Solana.Unity.Soar.Accounts;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject currentScene = null;

    public GameObject fightingScene;
    public GameObject inventoryScene;
    public GameObject blackSmithScene;
    public GameObject statsScene;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchScene(GameObject newScene)
    {
        if (currentScene != null)
            currentScene.SetActive(false);
        currentScene = newScene;
        currentScene.SetActive(true);

        {
            var invScene = currentScene.GetComponent<InventoryScene>();
            if (invScene != null)
            {
                invScene.init();
            }
        }
    }
    
    
    
}
