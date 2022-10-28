using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;



public class Gold : MonoBehaviour
{
    public static int goldObtained = 0;
    public TMP_Text textNoCollected;
    public int goldAmount;
    public float cropsCost;

    private void Update()
    {
        textNoCollected.text = "Gold : " + ShopManagerScript.goldObtained;
        if (goldObtained < -20)
        {
            SceneManager.LoadScene("Debt");

        }
    }


    // Start is called before the first frame update
    public void addGold()
    {
        // ShopManagerScript.goldObtained += goldAmount;
        //  textNoCollected.text = "Gold : " + ShopManagerScript.goldObtained;
        /*if (ShopManagerScript.crops !> goldLimit)
         {
        }*/
        if(ShopManagerScript.crops !>= cropsCost)
            ShopManagerScript.goldObtained += goldAmount;
        

    }
}
