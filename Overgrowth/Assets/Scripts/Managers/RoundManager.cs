using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    // Basic book keeping
    public int roundOn;

    // Timer stuff
    public int remainingTime;
    public TextMeshProUGUI timerText;

    // Round data
    public List<int> roundLengths;
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
        
    }

    public IEnumerator roundTimer(int length)
    {
        for(int i = 0; i < length; i++)
        {
            yield return new WaitForSeconds(1);
            remainingTime--;
            timerText.text = "" + remainingTime;
        }
    }

    // SHOP BUTTON FUNCTIONS
    
}
