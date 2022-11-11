using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerScript : MonoBehaviour
{

    public int hungerBar;
    public int maxHunger;
    public int hungerUsage;
    private ShopManagerScript shopManager;

    public Button myButton;

    private void Start()
    {
        shopManager = GetComponent<ButtonInfo>().ShopManager;
    }

    public void BoughtFood()
    {
        if(hungerBar < maxHunger)
            hungerBar += hungerUsage;

    }

    public void EndOfSale()
    {
        

        hungerBar -= hungerUsage;
        if(hungerBar <= 0)
        {
            myButton.image.color = Color.black;
            myButton.enabled = false;
            shopManager.peopleNames[3, 3] -= shopManager.peopleNames[3, GetComponent<ButtonInfo>().PersonID];
        }

    }

}
