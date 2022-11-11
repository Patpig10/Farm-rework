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

    private List<Vector2> points = new List<Vector2>();
    private Vector2 closestPoint;
    
    

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

        for(int x = 0; x < 8; x++)
            for(int z = 0; z < 8; z++)
            { 
                Vector2 newPoint = new Vector2(transform.position.x + 17.5f -(x * 5), transform.position.z - 12.8f +(z * 3.6f));

                points.Add(newPoint);
            }

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
                Vector2 thisPoint = NearestPoint(rayHit.point);
                    GameObject newPlot = GameObject.Instantiate(farmPlotPrefab, new Vector3(thisPoint.x, 1, thisPoint.y), transform.rotation);
                    newPlot.name = "Tilled";
                    farmPlots.Add(newPlot.GetComponent<FarmPlot>());
                break;
            case State.Seed:
                if (!rayHit.transform.gameObject.GetComponent<FarmPlot>().seeded.activeInHierarchy && ShopManagerScript.seed > 0)
                {

                    if (!rayHit.transform.gameObject.GetComponent<FarmPlot>().seeded.activeInHierarchy)
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
                    ShopManagerScript.crops += Random.Range(1, 4);
                    
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
        if(daysPassed > 5)
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
    public void ReturnFromMarket()
    {
        market.SetActive(false);
        toMarket = false;
    }

    public Vector2 NearestPoint(Vector3 point)
    {

        Vector2 thisPoint = new Vector2(0,0);
        point.y = 0;
        foreach(Vector2 location in points)
        {
            if (thisPoint.x == 0 && thisPoint.y == 0)
                thisPoint = location;
            else if (DistanceBetween(point, location) < DistanceBetween(point, thisPoint))
                thisPoint = location;
        }
        return thisPoint;
    }

    public float DistanceBetween(Vector3 firstPoint, Vector2 secondPoint)
    {
        float distance = 0;

        distance = Vector3.Distance(firstPoint, new Vector3(secondPoint.x, 0,secondPoint.y));

        return distance;
    }
}