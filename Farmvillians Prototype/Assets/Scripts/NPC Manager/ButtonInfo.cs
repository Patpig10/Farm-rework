using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonInfo : MonoBehaviour
{
    public int PersonID;
    public TMP_Text DemandTxt;
    public TMP_Text PaymentTxt;
    public ShopManagerScript ShopManager;
    // Update is called once per frame
    void Update()
    {
        DemandTxt.text = "Sell: " + ShopManager.peopleNames[2, PersonID].ToString() + " crops";
        PaymentTxt.text = "For: " + ShopManager.peopleNames[3, PersonID].ToString() + " Gold";
    }
}
