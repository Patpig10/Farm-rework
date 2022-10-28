using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TextCore;

public class FarmingControl : MonoBehaviour
{

    [Header("References")]
    public GameObject farmPlotPrefab;
    private List<FarmPlot> farmPlots = new List<FarmPlot>();
    public Camera mainCamera;
    public GameObject market;

    [Header("Plot Types")]
    private string changeThis;
    public enum State
    {
        Hoe,
        Seed,
        Harvest
    }

    public State state;

    [Header("Public Info")]
    private int excessCrops;
    public bool toMarket = false;
    public int daysPassed;
    public int goldPerDay;
    public int currentGold;
    public int seedCost;
    public int seedPurchaseAmount;

    [Header("Blight Stuff")]
    public int maxBlightChance;
    private float blightChance = 0;


    [Header("UI Ref")]
    public Button hoeUI;
    public Button harvestUI;
    public Button seedUI;
    
    public TMPro.TextMeshProUGUI seedCount;

    
    private RaycastHit rayHit;

    

    IEnumerator HoeState()
    {
        Debug.Log("Hoe: Mode Enabled");
        while (state == State.Hoe)
        {
            changeThis = "Ground";
            hoeUI.image.color = Color.gray;
            seedUI.image.color = Color.white;
            harvestUI.image.color = Color.white;
            yield return 0;
        }
        Debug.Log("Hoe: Mode Disabled");
        NextState();
    }

    IEnumerator SeedState()
    {
        Debug.Log("Seed: Mode Enabled");
        while (state == State.Seed)
        {
            changeThis = "Tilled";
            hoeUI.image.color = Color.white;
            seedUI.image.color = Color.gray;
            harvestUI.image.color = Color.white;
            yield return 0;
        }
        Debug.Log("Seed: Mode Disabled");
        NextState();
    }

    IEnumerator HarvestState()
    {
        Debug.Log("Harvest: Mode Enabled");
        while (state == State.Harvest)
        {
            changeThis = "Grown";
            hoeUI.image.color = Color.white;
            seedUI.image.color = Color.white;
            harvestUI.image.color = Color.gray;
            yield return 0;
        }
        Debug.Log("Harvest: Mode Disabled");
        NextState();
    }

    void Start()
    {
        ShopManagerScript.goldObtained = 30;
        NextState();
    }

    void NextState()
    {
        string methodName = state.ToString() + "State";
        System.Reflection.MethodInfo info =
            GetType().GetMethod(methodName,
                                System.Reflection.BindingFlags.NonPublic |
                                System.Reflection.BindingFlags.Instance);
        StartCoroutine((IEnumerator)info.Invoke(this, null));
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0) && !toMarket)
        {
            if(Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out rayHit))
            {
                Debug.Log(rayHit.transform.gameObject.tag + " " + changeThis);
                UseTool();
            }
        }

    }

    private void UseTool()
    {
        if (rayHit.collider.gameObject.tag != changeThis)
            return;
        Debug.Log("Correct layer");
        switch (state)
        {
            case State.Hoe:
                    GameObject newPlot = GameObject.Instantiate(farmPlotPrefab, rayHit.point, transform.rotation);
                    newPlot.name = "Tilled";
                    farmPlots.Add(newPlot.GetComponent<FarmPlot>());
                break;
            case State.Seed:
                if (!rayHit.transform.gameObject.GetComponent<FarmPlot>().seeded.activeInHierarchy && ShopManagerScript.seed > 0)
                {

                    if (!rayHit.transform.gameObject.GetComponent<FarmPlot>().seeded.active)
                    {
                        rayHit.transform.gameObject.GetComponent<FarmPlot>().seeded.SetActive(true);
                        ShopManagerScript.seed--;
                    }
                }
                break;
            case State.Harvest:
                if (rayHit.transform.parent.gameObject.GetComponent<FarmPlot>().grown.activeInHierarchy)
                {
                    rayHit.transform.parent.gameObject.GetComponent<FarmPlot>().grown.SetActive(false);
                    ShopManagerScript.crops += Random.Range(2, 5);
                    
                }
                break;
            default:
                Debug.Log("Failed to be state");
                break;
        }

    }

    public void HoeMode()
    {
        state = State.Hoe;
    }

    public void SeedMode()
    {
        state = State.Seed;
    }
    public void HarvestMode()
    {
        state = State.Harvest;
    }


    private bool AreaClear(Vector3 point)
    {

        bool areaClear = true;

        foreach(FarmPlot plot in farmPlots)
        {
            if(Vector3.Distance(new Vector3 (point.x, 0, 0), new Vector3(transform.position.x, 0, 0)) >= 5
                && Vector3.Distance(new Vector3(0, 0, point.z), new Vector3(0, 0, transform.position.z)) >= 3.4f)
            {
                areaClear =  false;
            }
        }

        return areaClear;
    }

    public void NextDay()
    {
        ShopManagerScript.crops -= excessCrops;
        toMarket = false;
        market.SetActive(false);
        daysPassed++;
        ShopManagerScript.goldObtained -= goldPerDay;




        foreach (FarmPlot plot in farmPlots) 
        {
            float generated = Random.Range(0, 100);

            if (plot.seeded.activeInHierarchy && generated > blightChance)
            {
                plot.seeded.SetActive(false);
                plot.grown.SetActive(true);

            }
            else if (plot.seeded.activeInHierarchy && generated <= blightChance)
            {
                plot.seeded.SetActive(false);
                plot.blighted.SetActive(true);

            }
            else
            {
                plot.blighted.SetActive(false);
                plot.grown.SetActive(false);
            }
        }

        blightChance += 10;
        if (blightChance > maxBlightChance)
            blightChance = maxBlightChance;

        excessCrops = ShopManagerScript.crops;
    }
    public void TurnCropToSeed()
    {
        if (ShopManagerScript.crops > 0)
        {
            ShopManagerScript.crops--;
            ShopManagerScript.seed++;
        }
    }
    public void TurnGoldToSeed()
    {
        if (ShopManagerScript.goldObtained > seedCost)
        {
            ShopManagerScript.seed += seedPurchaseAmount;
            ShopManagerScript.goldObtained -= seedCost;
        }
    }

    public void ToMarket()
    {

        market.SetActive(true);
        toMarket = true;
    }


}
