using System.Collections.Generic;
using UnityEngine;

// Stors all data for the game, some simple book keeping functions
public class GameData : MonoBehaviour
{
    // Singleton
    public static GameData Instance;

    // Round variables
    public int round;

    // Player inventory variables

    // 0: Wood
    // 1: Petals
    // 2: Grass
    public List<int> resources = new List<int>(3);
    public int money;

    public List<int> plantCount;

    public PlayerMovement player;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    // Round management
    public void startGame()
    {
        round = 0;
        money = 0;
        for(int i = 0; i < 3; i++)
        {
            resources[i] = 0;
        }

        for(int i = 0; i < 4; i++)
        {
            plantCount[i] = 0;
        }

        player = GameObject.Find("PlayerSprite").GetComponent<PlayerMovement>();
    }

    // Book keeping
    public void addToInventory(int index, int amount)
    {
        resources[index] += amount;
    }

    public void addPlant(int index, PlantData pd)
    {
        plantCount[index]++;
        if(pd.effect == "attack")
        {
            player.damage += pd.effectAmount;
        }
    }

    public void removePlant(int index, PlantData pd)
    {
        plantCount[index]--;
        if(pd.effect == "attack")
        {
            player.damage -= pd.effectAmount;
        }
    }
}
