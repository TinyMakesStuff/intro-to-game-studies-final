using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance;
    // Basic book keeping
    [Header("BOOK KEEPING")]
    public int roundOn;
    public List<PlantData> plantInfo;
    public PlayerMovement playerMovement;

    // Timer stuff
    [Header("TIMER")]
    public int remainingTime;
    public TextMeshProUGUI timerText;

    // Round data
    [Header("ROUNDS")]
    public List<Round> rounds;
    public List<GameObject> curEnemies;

    // Shop vars
    [Header("SHOP VARIABLES")]
    public GameObject shopPanel;
    public List<int> costs;
    public List<UnityEngine.UI.Image> images;
    public List<TextMeshProUGUI> buttonText;
    public List<string> types;
    public List<int> itemIndex;
    public List<int> amounts;
    public List<string> resourceNames;
    public TextMeshProUGUI moneyText;
    public List<Sprite> resourceIcons;
    public List<Sprite> plantIcons;
    public GameObject inventoryPanel;


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

    public void startRound()
    {
        remainingTime = rounds[roundOn].roundLength;
        StartCoroutine(roundTimer(rounds[roundOn].roundLength));
        StartCoroutine(spawnBlobs());
    }

    public void goToShop()
    {
        shopPanel.SetActive(true);
        inventoryPanel.SetActive(false);

        playerMovement.canBuild = false;
        playerMovement.canMove = false;

        moneyText.text = "Gold: " + GameData.Instance.money;

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
                playerMovement.updateInventory();
                images[i].sprite = plantIcons[itemIndex[i]];
            } else
            {
                // is a resource
                types[i] = "resource";
                itemIndex[i] = Random.Range(0, 3);
                amounts[i] = Random.Range(1, 6);
                costs[i] = amounts[i] * 2;

                buttonText[i].text = amounts[i] + " " + resourceNames[itemIndex[i]] + " - " + costs[i] + " gold";
                playerMovement.updateInventory();
                images[i].sprite = resourceIcons[itemIndex[i]];
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

        if(roundOn >= 8)
        {
            SceneManager.LoadScene(0);
        }
        // TODO: Add code to end the game HERE
        clearEnemies();
        goToShop();
    }

    public IEnumerator spawnBlobs()
    {
        Round cur = rounds[roundOn];

        for(int i = 0; i < cur.blobCount; i++)
        {
            Blob curBlob = cur.blobs[i];

            for(int j = 0; j < curBlob.typeCount; j++)
            {
                if(curBlob.spawnType[j] == "scatter")
                {
                    // TODO: add spawning code for scattering them randomly around the map
                    for(int k = 0; k < curBlob.amount[j]; k++)
                    {
                        GameObject newEnemy = Instantiate(curBlob.enemies[j]);
                        newEnemy.transform.position = new Vector3(Random.Range(-60, 8), 1, Random.Range(-40, 5));
                        curEnemies.Add(newEnemy);
                    }
                } else if(curBlob.spawnType[j] == "point")
                {
                    // TODO: add spawning code for spawning them around a set point
                    for(int k = 0; k < curBlob.amount[j]; k++)
                    {
                        GameObject newEnemy = Instantiate(curBlob.enemies[j]);
                        newEnemy.transform.position = new Vector3(Random.Range(curBlob.xCenter[j] - 3, curBlob.xCenter[j] + 3), 1, Random.Range(curBlob.zCenter[j] - 3, curBlob.xCenter[j] + 3));
                        curEnemies.Add(newEnemy);
                    }
                }
            }

            yield return new WaitForSeconds(cur.blobLengths[i]);
        }
    }

    public void clearEnemies()
    {
        for(int i = 0; i < curEnemies.Count; i++)
        {
            Destroy(curEnemies[i]);
        }

        curEnemies = new List<GameObject>();
    }

    // SHOP BUTTON FUNCTIONS
    public void exitShop()
    {
        playerMovement.canBuild = true;
        playerMovement.canMove = true;
        shopPanel.SetActive(false);
        inventoryPanel.SetActive(true);
        startRound();
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

        moneyText.text = "Gold: " + GameData.Instance.money;
        playerMovement.updateInventory();
    }
}
