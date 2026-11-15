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
        resources = new List<int>(3);
    }

    // Book keeping
    public void addToInventory(int index, int amount)
    {
        resources[index] += amount;
    }
}
