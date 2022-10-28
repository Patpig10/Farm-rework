using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerScript : MonoBehaviour
{

    public int hungerBar;
    public int maxHunger;
    public int hungerUsage;

    public Button myButton;

    public void BoughtFood()
    {
        if(hungerBar < maxHunger)
            hungerBar++;

    }

    public void EndOfSale()
    {
        hungerBar -= hungerUsage;
        if(hungerBar <= 0)
        {
            myButton.image.color = Color.black;
            myButton.enabled = false;
        }

    }

}
