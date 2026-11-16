using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance;
    // Basic book keeping
    public int roundOn;
    public List<PlantData> plantInfo;

    // Timer stuff
    public int remainingTime;
    public TextMeshProUGUI timerText;

    // Round data
    public List<int> roundLengths;

    // Shop vars
    public GameObject shopPanel;
    public List<int> costs;
    public List<UnityEngine.UI.Image> images;
    public List<TextMeshProUGUI> buttonText;
    public List<string> types;
    public List<int> itemIndex;
    public List<int> amounts;
    public List<string> resourceNames;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startRound(int length)
    {
        remainingTime = length;
        StartCoroutine(roundTimer(length));
    }

    public void goToShop()
    {
        shopPanel.SetActive(true);

        for(int i = 0; i < images.Count; i++)
        {
            if(Random.Range(0, 2) == 0)
            {
                // is a plant
                types[i] = "plant";
                itemIndex[i] = Random.Range(0, plantInfo.Count);
                amounts[i] = 1;
                costs[i] = plantInfo[itemIndex[i]].cost;

                buttonText[i].text = plantInfo[itemIndex[i]].plant + " - " + costs[i] + " gold";
            } else
            {
                // is a resource
                types[i] = "resource";
                itemIndex[i] = Random.Range(0, 3);
                amounts[i] = Random.Range(1, 6);
                costs[i] = amounts[i] * 2;

                buttonText[i].text = amounts[i] + " " + resourceNames[itemIndex[i]] + " - " + costs[i] + " gold";
            }
        }
    }

    public IEnumerator roundTimer(int length)
    {
        timerText.text = "" + remainingTime;
        
        for(int i = 0; i < length; i++)
        {
            yield return new WaitForSeconds(1);
            remainingTime--;
            timerText.text = "" + remainingTime;
        }

        roundOn++;
        goToShop();
    }

    // SHOP BUTTON FUNCTIONS
    public void exitShop()
    {
        shopPanel.SetActive(false);
        startRound(10);
    }

    public void buyItem(int button)
    {
        if(GameData.Instance.money >= costs[button])
        {
            GameData.Instance.money -= costs[button];

            if(types[button] == "plant")
            {
                GameData.Instance.addPlant(itemIndex[button], plantInfo[itemIndex[button]]);
            } else if(types[button] == "resource")
            {
                GameData.Instance.addToInventory(itemIndex[button], amounts[button]);
            }
        }
    }
}
