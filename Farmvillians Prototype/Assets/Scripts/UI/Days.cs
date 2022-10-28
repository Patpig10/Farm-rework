using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;


public class Days : MonoBehaviour
{
    public TMP_Text Day;
    public static int days = 1;
    public static int debt = -20;


    // Start is called before the first frame update
    public void canlender()
    {
        days++;
        Day.text = "Days: " + days;
    }
    public void Update()
    {

        if (ShopManagerScript.goldObtained < debt)
        {
            SceneManager.LoadScene(2);

        }
        Day.text = "Days: " + days;
        if( days == 15)
        {
            SceneManager.LoadScene("GameWin");

        }
        
    }
}
