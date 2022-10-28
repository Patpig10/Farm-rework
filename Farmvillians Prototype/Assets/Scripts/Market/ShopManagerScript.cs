using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class ShopManagerScript : MonoBehaviour
{
    public int[,] peopleNames = new int[4, 4];
    public static int crops = 0;
    public static int seed = 6;
    public TMP_Text CropsTxt;
    public TMP_Text seedText;
    public static int goldObtained = 0; //can be put in player code but you must change some code in gold script
    public float cropsCost;
    public float goldLimit;



    void Start()
    {
        CropsTxt.text = "Crops:" + crops;

        //people
        peopleNames[1, 1] = 1;
        peopleNames[1, 2] = 2;
        peopleNames[1, 3] = 3;

        //demands
        peopleNames[2, 1] = 2;
        peopleNames[2, 2] = 2;
        peopleNames[2, 3] = 3;

        //payment
        peopleNames[3, 1] = 2;
        peopleNames[3, 2] = 1;
        peopleNames[3, 3] = 5;
    }

    private void Update()
    {

        CropsTxt.text = "Crops: " + crops;
        seedText.text = "Seeds: " + seed;

        

    }

    public void Sell()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Events").GetComponent<EventSystem>().currentSelectedGameObject;
        if (crops >= peopleNames[2, ButtonRef.GetComponent<ButtonInfo>().PersonID])
        {
            crops -= peopleNames[2, ButtonRef.GetComponent<ButtonInfo>().PersonID];
            ButtonRef.GetComponent<ButtonInfo>().PaymentTxt.text = peopleNames[3, ButtonRef.GetComponent<ButtonInfo>().PersonID].ToString();

        }
        
    }


    public void AddSeedForGold()
    {
        if (goldObtained! >= goldLimit)
        {
            goldObtained -= 3;
      
            seed += 5;
        }

    }

    public void AddSeedForCrop()
    {
        if (crops! >= cropsCost)
        {
            crops--;
        
        
            seed++;
        }

    }
    
}
